using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System;

namespace SasFileStorage.Models
{
    public class ModeloApi
    {
        public Uri GenerarTokenFile()
        {
            string clave = CloudConfigurationManager.GetSetting("cadenaConexion");
            CloudStorageAccount cuenta = CloudStorageAccount.Parse(clave);
            CloudFileClient cliente = cuenta.CreateCloudFileClient();
            CloudFileShare recursoCompartido = cliente.GetShareReference("direjemplo");

            SharedAccessFilePolicy permisos = new SharedAccessFilePolicy();
            permisos.SharedAccessExpiryTime = DateTime.Now.AddMinutes(5);
            permisos.Permissions = SharedAccessFilePermissions.Create | SharedAccessFilePermissions.Delete |
                SharedAccessFilePermissions.List | SharedAccessFilePermissions.Read | SharedAccessFilePermissions.Write;
            
            string strToken = recursoCompartido.GetSharedAccessSignature(permisos);
            string aux = recursoCompartido.Uri + strToken;
            Uri token = new Uri(aux);
            return token;
        }
    }
}
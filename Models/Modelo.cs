using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.Web;

namespace SasFileStorage.Models
{
    public class Modelo
    {
        ModeloApi modeloapi;

        public Modelo()
        {
            modeloapi = new ModeloApi();
        }

        public void CrearFichero(HttpPostedFileBase fichero)
        {
            Uri token = modeloapi.GenerarTokenFile();
            CloudFileShare recursoCompartido = new CloudFileShare(token);
            CloudFileDirectory Directorio = recursoCompartido.GetRootDirectoryReference();

            string nombre = fichero.FileName;
            if (nombre != "ficheromuestra.txt")
            {
                CloudFile file = Directorio.GetFileReference(nombre);
                file.UploadFromStream(fichero.InputStream);
            }
        }

        public List<string> GetListadoFicheros()
        {
            Uri token = modeloapi.GenerarTokenFile();
            CloudFileShare recursoCompartido = new CloudFileShare(token);
            CloudFileDirectory Directorio = recursoCompartido.GetRootDirectoryReference();

            IEnumerable<IListFileItem> lista = Directorio.ListFilesAndDirectories();
            List<string> datos = new List<string>();
            foreach (IListFileItem i in lista)
            {
                string aux = i.Uri.ToString();
                int pos = aux.LastIndexOf('/') + 1;
                aux = aux.Substring(pos);
                datos.Add(aux);
            }

            return datos;
        }

        public void EliminarFichero(string nombre)
        {
            Uri token = modeloapi.GenerarTokenFile();
            CloudFileShare recursoCompartido = new CloudFileShare(token);
            CloudFileDirectory Directorio = recursoCompartido.GetRootDirectoryReference();
            
            CloudFile file = Directorio.GetFileReference(nombre);
            file.DeleteIfExists();
        }
    }
}
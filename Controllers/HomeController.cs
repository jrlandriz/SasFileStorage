using SasFileStorage.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Web;

namespace SasFileStorage.Controllers
{
    public class HomeController : Controller
    {
        Modelo modelo;

        public HomeController()
        {
            modelo = new Modelo();
        }

        // GET: Index
        [HttpGet]
        public ActionResult Index(string accion, string fichero)
        {
            if(accion == "eliminar")
            {
                modelo.EliminarFichero(fichero);
            }

            List<string> ficheros = modelo.GetListadoFicheros();
            ViewBag.Ficheros = ficheros;


            return View();
        }

        // POST: Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string accion, HttpPostedFileBase ficheroSubir)
        {
            List<string> ficheros = modelo.GetListadoFicheros();
            ViewBag.Ficheros = ficheros;

            if (accion == "muestra")
            {
                // Recuperamos las claves de acceso
                string clave = CloudConfigurationManager.GetSetting("cadenaConexion");

                // Creamos un objeto para acceder a la cuenta Storage
                CloudStorageAccount cuenta = CloudStorageAccount.Parse(clave);

                // Creamos un cliente del tipo de recurso que deseamos leer (File)
                CloudFileClient cliente = cuenta.CreateCloudFileClient();

                // Accedemos al recurso compartido que deseemos
                CloudFileShare recurso = cliente.GetShareReference("direjemplo");

                // Nos posicionamos en la raiz del recurso compartido
                CloudFileDirectory directorio = recurso.GetRootDirectoryReference();

                // Accedemos al fichero por su nombre
                CloudFile fichero = directorio.GetFileReference("ficheromuestra.txt");

                // Leemos el fichero de forma asincrona
                string texto = await fichero.DownloadTextAsync();
                ViewBag.Texto = texto;
            }
            else if (accion == "subir")
            {
                modelo.CrearFichero(ficheroSubir);
            }

            return View();
        }
    }
}
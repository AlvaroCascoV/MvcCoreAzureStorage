using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;

namespace MvcCoreAzureStorage.Services
{
    public class ServiceStorageFiles
    {
        //TODO SERVICIO STORAGE TRABAJA SIEMPRE CON UN CLIENT
        //DICHO CLIENTE PUEDE SER DIRECTAMENTE UN SHARED O 
        //PODRIA SER EL SERVICIO DE FILES
        private ShareDirectoryClient root;

        public ServiceStorageFiles(IConfiguration configuration)
        {
            string azureKeys = configuration.GetValue<string>("AzureKeys:StorageAccount");
            //NUESTRO CLIENTE TABAJARÁ SOBRE EL SHARED
            //QUE HEMOS CREADO PREVIAMENTE (ejemplo)
            ShareClient client = new ShareClient(azureKeys, "ejemplo");
            this.root = client.GetRootDirectoryClient();
        }

        //LISTAR TODOS LOS FICHEROS
        public async Task<List<string>> GetFilesAsync()
        {
            List<string> files = new List<string>();
            await foreach (ShareFileItem item in this.root.GetFilesAndDirectoriesAsync())
            {
                files.Add(item.Name);
            }
            return files;
        }

        //LEER CONTENIDO DE UN FICHERO
        public async Task<string> ReadFileAsync(string fileName)
        {
            ShareFileClient fileClient = this.root.GetFileClient(fileName);
            ShareFileDownloadInfo data = await fileClient.DownloadAsync();
            Stream stream = data.Content;
            string contenido = "";
            using (StreamReader reader = new StreamReader(stream))
            {
                contenido = await reader.ReadToEndAsync();
            }
            return contenido;
        }

        //subida de ficheros
        public async Task UploadFileAsync(string fileName, Stream stream)
        {
            ShareFileClient fileClient = this.root.GetFileClient(fileName);
            await fileClient.CreateAsync(stream.Length);
            await fileClient.UploadAsync(stream);
        }

        //eliminar un fichero
        public async Task DeleteFileAsync(string fileName)
        {
            ShareFileClient fileClient = this.root.GetFileClient(fileName);
            await fileClient.DeleteAsync();
        }
    }
}

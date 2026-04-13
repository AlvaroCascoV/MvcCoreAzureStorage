using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MvcCoreAzureStorage.Models;

namespace MvcCoreAzureStorage.Services
{
    public class ServiceStorageBlobs
    {
        private BlobServiceClient client;

        public ServiceStorageBlobs(BlobServiceClient client)
        {
            this.client = client;
        }

        //METODO PARA RECUPERAR TODOS LOS CONTAINERS
        public async Task<List<string>> GetContainersAsync()
        {
            List<string> containers = new List<string>();
            await foreach (BlobContainerItem item in this.client.GetBlobContainersAsync())
            {
                containers.Add(item.Name);
            }
            return containers;
        }

        //METODO PARA CREAR UN CONTAINER
        public async Task CreateContainerAsync(string containerName)
        {
            await this.client.CreateBlobContainerAsync(containerName.ToLower(), PublicAccessType.None); //se cambia el tipo del container para controlar el acceso
        }

        //ELIMINAR UN CONTAINER
        public async Task DeleteContainerAsync(string containerName)
        {
            await this.client.DeleteBlobContainerAsync(containerName);
        }

        //LISTADO DE FICHEROS DENTRO DE UN CONTAINER
        public async Task<List<BlobModel>> GetBlobsAsync(string containerName)
        {
            //NECESITAMOS UN CLIENTE DE BLOBS CONTAINER
            //PARA EL ACCESO A LOS FICHEROS
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerName);
            List<BlobModel> blobs = new List<BlobModel>();
            await foreach (BlobItem item in containerClient.GetBlobsAsync())
            {
                //necesito este otro client para recuperar la url del blob
                BlobClient blobClient = containerClient.GetBlobClient(item.Name);
                BlobModel model = new BlobModel();
                model.Nombre = item.Name;
                model.Container = containerName;
                model.Url = blobClient.Uri.AbsoluteUri;
                blobs.Add(model);
            }
            return blobs;
        }

        //METODO PARA ELIMINAR UN BLOB
        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            BlobContainerClient blobContainerClient = this.client.GetBlobContainerClient(containerName);
            await blobContainerClient.DeleteBlobAsync(blobName);
        }

        //SUBIR UN BLOB A UN CONTAINER
        public async Task UploadBlobAsync(string containerName, string blobName, Stream stream)
        {
            BlobContainerClient blobContainerClient = this.client.GetBlobContainerClient(containerName);
            await blobContainerClient.UploadBlobAsync(blobName, stream);
        }
    }
}

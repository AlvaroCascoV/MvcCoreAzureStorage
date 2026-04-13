using Microsoft.AspNetCore.Mvc;
using MvcCoreAzureStorage.Models;
using MvcCoreAzureStorage.Services;
using System.Threading.Tasks;

namespace MvcCoreAzureStorage.Controllers
{
    public class AzureBlobsController : Controller
    {
        private ServiceStorageBlobs service;

        public AzureBlobsController(ServiceStorageBlobs service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<string> containers = await this.service.GetContainersAsync();
            return View(containers);
        }

        public IActionResult CreateContainer()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateContainer(string containerName)
        {
            await this.service.CreateContainerAsync(containerName);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteContainer(string containerName)
        {
            await this.service.DeleteContainerAsync(containerName);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListBlobs(string containerName)
        {
            ViewData["CONTAINER"] = containerName;
            List<BlobModel> models = await this.service.GetBlobsAsync(containerName);
            return View(models);
        }

        public IActionResult UploadBlob(string containerName)
        {
            ViewData["CONTAINER"] = containerName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadBlob(string containerName, IFormFile file)
        {
            string blobName = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.service.UploadBlobAsync(containerName, blobName, stream);
            }
            return RedirectToAction("ListBlobs", new { containerName = containerName });
        }

        public async Task<IActionResult> DeleteBlob(string containerName, string blobName)
        {
            await this.service.DeleteBlobAsync(containerName, blobName);
            return RedirectToAction("ListBlobs", new { containerName = containerName });
        }
        //descargar el blob para mostrarlo en la vista, si el container es privado
        public async Task<IActionResult> VerImagen(string containerName, string blobName)
        {
            Stream stream = await this.service.GetBlobStreamAsync(containerName, blobName);
            return File(stream, "image/jpeg");
        }
    }
}

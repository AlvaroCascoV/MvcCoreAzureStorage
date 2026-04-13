using Microsoft.AspNetCore.Mvc;
using MvcCoreAzureStorage.Services;

namespace MvcCoreAzureStorage.Controllers
{
    public class AzureFilesController : Controller
    {
        private ServiceStorageFiles service;

        public AzureFilesController(ServiceStorageFiles service)
        {
            this.service = service;
        }
        public async Task<IActionResult> Index()
        {
            List<string> files = await this.service.GetFilesAsync();
            return View(files);
        }
        public async Task<IActionResult> ReadFile(string fileName)
        {
            string data = await this.service.ReadFileAsync(fileName);
            ViewData["DATA"] = data;
            return View();
        }
        public async Task<IActionResult> DeleteFile(string fileName)
        {
            await this.service.DeleteFileAsync(fileName);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UploadFile()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            string fileName = file.FileName;
            //DEBERIAMOS TENER UN HELPER QUE SE ENCARGUE
            //DE "LIMPIAR" EL fileName
            using (Stream stream = file.OpenReadStream())
            {
                await this.service.UploadFileAsync(fileName, stream);
            }
            ViewData["MENSAJE"] = "Fichero subido correctamente";
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ABCRetailStorageApp.Services;

namespace ABCRetailStorageApp.Controllers
{
    public class FilesController : Controller
    {
        private readonly FileStorageService _service;
        public FilesController(FileStorageService service) => _service = service;

        public async Task<IActionResult> Index() => View(await _service.GetFilesAsync());// GetFilesAsync(); is RED

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null) await _service.UploadFileAsync(file);// UploadFileAsync(file); is RED
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string name)
        {
            await _service.DeleteFileAsync(name);
            return RedirectToAction("Index");
        }
    }
}

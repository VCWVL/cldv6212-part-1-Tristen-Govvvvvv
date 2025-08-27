using Microsoft.AspNetCore.Mvc;
using ABCRetailStorageApp.Services;

namespace ABCRetailStorageApp.Controllers
{
    public class QueuesController : Controller
    {
        private readonly QueueStorageService _queue;

        public QueuesController() => _queue = new QueueStorageService();

        // Display messages
        public async Task<IActionResult> Index()
        {
            var messages = await _queue.PeekMessagesAsync(); // peek instead of receive
            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> Send(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                await _queue.EnqueueAsync(message);
            }
            return RedirectToAction("Index");
        }
    }
}

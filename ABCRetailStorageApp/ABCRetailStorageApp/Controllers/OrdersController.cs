using Microsoft.AspNetCore.Mvc;
using ABCRetailStorageApp.Models;
using ABCRetailStorageApp.Services;

namespace ABCRetailStorageApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly TableStorageService _tables;
        private readonly QueueStorageService _queue;

        public OrdersController()
        {
            _tables = new TableStorageService();
            _queue = new QueueStorageService();
        }

        // GET: Orders
        public async Task<IActionResult> Index() => View(await _tables.GetOrdersAsync());

        // GET: Orders/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Customers = await _tables.GetCustomersAsync();
            ViewBag.Products = await _tables.GetProductsAsync();
            return View(new OrderEntity()); // now view is just "Create.cshtml"
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderEntity model)
        {
            // Load dropdowns again in case of validation errors
            ViewBag.Customers = await _tables.GetCustomersAsync();
            ViewBag.Products = await _tables.GetProductsAsync();

            if (!ModelState.IsValid)
                return View(model);

            // Get customer and product from storage
            var customer = await _tables.GetCustomerAsync(model.CustomerRowKey);
            var product = await _tables.GetProductAsync(model.ProductRowKey);

            if (customer == null || product == null)
            {
                ModelState.AddModelError("", "Selected customer or product does not exist.");
                return View(model);
            }

            // Assign values automatically from product and customer
            model.CustomerName = customer.Name;
            model.ProductName = product.Name;
            model.UnitPrice = product.Price;          // Automatically use product price
            model.TotalPrice = model.UnitPrice * model.Quantity;

            // Save order to Table Storage
            await _tables.UpsertOrderAsync(model);

            // Send message to queue (optional)
            string message = $"New order: {model.RowKey} for {model.CustomerName} -> {model.ProductName} x {model.Quantity}";
            await _queue.EnqueueAsync(message);

            return RedirectToAction(nameof(Index)); // redirect to Orders index
        }

        // GET: Orders/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var order = await _tables.GetOrderAsync(id);
            if (order == null) return NotFound();

            ViewBag.Customers = await _tables.GetCustomersAsync();
            ViewBag.Products = await _tables.GetProductsAsync();

            return View("Edit", order);
        }

        // POST: Orders/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrderEntity model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Customers = await _tables.GetCustomersAsync();
                ViewBag.Products = await _tables.GetProductsAsync();
                return View("Edit", model);
            }

            var customer = await _tables.GetCustomerAsync(model.CustomerRowKey);
            var product = await _tables.GetProductAsync(model.ProductRowKey);

            model.CustomerName = customer?.Name ?? "Unknown";
            model.ProductName = product?.Name ?? "Unknown";
            model.UnitPrice = product?.Price ?? 0;
            model.TotalPrice = model.UnitPrice * model.Quantity;

            await _tables.UpsertOrderAsync(model);

            return RedirectToAction(nameof(Index));
        }

        // GET: Orders/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var order = await _tables.GetOrderAsync(id);
            if (order == null) return NotFound();
            return View("Delete", order);
        }

        // POST: Orders/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(string rowKey)
        {
            await _tables.DeleteOrderAsync(rowKey);
            return RedirectToAction(nameof(Index));
        }
    }
}

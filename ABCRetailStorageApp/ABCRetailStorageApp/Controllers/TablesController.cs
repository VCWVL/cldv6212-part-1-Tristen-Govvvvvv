using Microsoft.AspNetCore.Mvc;
using ABCRetailStorageApp.Models;
using ABCRetailStorageApp.Services;

namespace ABCRetailStorageApp.Controllers
{
    public class TablesController : Controller
    {
        private readonly TableStorageService _tables;
        private readonly BlobStorageService _blobs;

        public TablesController()
        {
            _tables = new TableStorageService();
            _blobs = new BlobStorageService();
        }

        // Customers
        public async Task<IActionResult> Customers() => View(await _tables.GetCustomersAsync());

        [HttpGet]
        public IActionResult CreateCustomer() => View(new CustomerEntity());

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CustomerEntity model)
        {
            if (!ModelState.IsValid) return View(model);
            await _tables.UpsertCustomerAsync(model);
            return RedirectToAction(nameof(Customers));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCustomer(string id) => View(await _tables.GetCustomerAsync(id));

        [HttpPost]
        public async Task<IActionResult> DeleteCustomerPost(string rowKey)
        {
            await _tables.DeleteCustomerAsync(rowKey);
            return RedirectToAction(nameof(Customers));
        }

        // Products
        public async Task<IActionResult> Products() => View(await _tables.GetProductsAsync());

        // GET: Create Product
        [HttpGet]
        public IActionResult CreateProduct() => View(new ProductEntity());

        // POST: Create Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductEntity model, IFormFile? image)
        {
            // Validate first
            if (!ModelState.IsValid)
            {
                return View(model); // reload page with errors
            }

            // Upload image if provided
            if (image != null)
            {
                model.ImageUrl = await _blobs.UploadBlobAsync(image);
            }

            // Save product
            await _tables.UpsertProductAsync(model);

            return RedirectToAction(nameof(Products)); // redirect to index
        }


        [HttpGet]
        public async Task<IActionResult> DeleteProduct(string id) => View(await _tables.GetProductAsync(id));

        [HttpPost]
        public async Task<IActionResult> DeleteProductPost(string rowKey)
        {
            await _tables.DeleteProductAsync(rowKey);
            return RedirectToAction(nameof(Products));
        }

        // ---------- EDIT CUSTOMER ----------
        [HttpGet]
        public async Task<IActionResult> EditCustomer(string id)
        {
            var customer = await _tables.GetCustomerAsync(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> EditCustomer(CustomerEntity model)
        {
            if (!ModelState.IsValid) return View(model);
            await _tables.UpsertCustomerAsync(model);
            return RedirectToAction(nameof(Customers));
        }

        // ---------- EDIT PRODUCT ----------
        // GET: Edit Product
        [HttpGet]
        public async Task<IActionResult> EditProduct(string id)
        {
            var product = await _tables.GetProductAsync(id);
            if (product == null) return NotFound();
            return View(product);
        }

        // POST: Edit Product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(ProductEntity model, IFormFile? image)
        {
            // Get the original product from storage
            var originalProduct = await _tables.GetProductAsync(model.RowKey);
            if (originalProduct == null) return NotFound();

            // Preserve ImageUrl if no new image uploaded
            if (image != null)
            {
                model.ImageUrl = await _blobs.UploadBlobAsync(image);
            }
            else
            {
                model.ImageUrl = originalProduct.ImageUrl;
            }

            // Preserve original price if form somehow cleared it
            if (model.Price <= 0)
            {
                model.Price = originalProduct.Price;
            }

            if (!ModelState.IsValid)
            {
                return View(model); // reload with validation errors
            }

            // Save updated product
            await _tables.UpsertProductAsync(model);

            return RedirectToAction(nameof(Products)); // redirect to product list
        }


    }
}

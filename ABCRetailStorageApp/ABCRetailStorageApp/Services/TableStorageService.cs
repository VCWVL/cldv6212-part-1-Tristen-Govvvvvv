using Azure.Data.Tables;
using ABCRetailStorageApp.Models;

namespace ABCRetailStorageApp.Services
{
    public class TableStorageService
    {
        private readonly TableClient _customerTable;
        private readonly TableClient _productTable;
        private readonly TableClient _orderTable;

        public TableStorageService(string? connectionString = null)
        {
            connectionString ??= "DefaultEndpointsProtocol=https;AccountName=st10446952storage;AccountKey=CzispX7cxkdyVi0pHfZzh0IHox/KzeRvMvhAePBzROQPrIEk4JaO+rsDFIcWQayJGl5ZxJLUUMFW+AStEgMXPQ==;EndpointSuffix=core.windows.net";

            _customerTable = new TableClient(connectionString, "Customers");
            _productTable = new TableClient(connectionString, "Products");
            _orderTable = new TableClient(connectionString, "Orders");

            _customerTable.CreateIfNotExists();
            _productTable.CreateIfNotExists();
            _orderTable.CreateIfNotExists();
        }

        // Customers
        public async Task<List<CustomerEntity>> GetCustomersAsync()
        {
            var list = new List<CustomerEntity>();
            await foreach (var entity in _customerTable.QueryAsync<CustomerEntity>()) list.Add(entity);
            return list;
        }

        public async Task<CustomerEntity?> GetCustomerAsync(string rowKey)
        {
            try { return (await _customerTable.GetEntityAsync<CustomerEntity>("Customer", rowKey)).Value; }
            catch { return null; }
        }

        public async Task UpsertCustomerAsync(CustomerEntity entity)
        {
            entity.PartitionKey = "Customer";
            await _customerTable.UpsertEntityAsync(entity);
        }

        public async Task DeleteCustomerAsync(string rowKey) => await _customerTable.DeleteEntityAsync("Customer", rowKey);

        // Products
        public async Task<List<ProductEntity>> GetProductsAsync()
        {
            var list = new List<ProductEntity>();
            await foreach (var entity in _productTable.QueryAsync<ProductEntity>()) list.Add(entity);
            return list;
        }

        public async Task<ProductEntity?> GetProductAsync(string rowKey)
        {
            try { return (await _productTable.GetEntityAsync<ProductEntity>("Product", rowKey)).Value; }
            catch { return null; }
        }

        public async Task UpsertProductAsync(ProductEntity entity)
        {
            entity.PartitionKey = "Product";
            await _productTable.UpsertEntityAsync(entity);
        }

        public async Task DeleteProductAsync(string rowKey) => await _productTable.DeleteEntityAsync("Product", rowKey);

        // Orders
        public async Task<List<OrderEntity>> GetOrdersAsync()
        {
            var list = new List<OrderEntity>();
            await foreach (var entity in _orderTable.QueryAsync<OrderEntity>()) list.Add(entity);
            return list;
        }

        public async Task<OrderEntity?> GetOrderAsync(string rowKey)
        {
            try { return (await _orderTable.GetEntityAsync<OrderEntity>("Order", rowKey)).Value; }
            catch { return null; }
        }

        public async Task UpsertOrderAsync(OrderEntity entity)
        {
            entity.PartitionKey = "Order";
            entity.TotalPrice = entity.UnitPrice * entity.Quantity;
            await _orderTable.UpsertEntityAsync(entity);
        }

        public async Task DeleteOrderAsync(string rowKey) => await _orderTable.DeleteEntityAsync("Order", rowKey);
    }
}

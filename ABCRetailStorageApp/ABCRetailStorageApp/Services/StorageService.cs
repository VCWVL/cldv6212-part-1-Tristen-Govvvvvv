namespace ABCRetailStorageApp.Services
{
    // Root service for shared config
    public class StorageService
    {
        public string ConnectionString { get; }
        public string BlobContainer { get; }
        public string CustomersTable { get; }
        public string ProductsTable { get; }
        public string OrdersQueue { get; }
        public string FileShare { get; }

        public StorageService(IConfiguration config)
        {
            ConnectionString = config["AzureStorage:ConnectionString"];
            BlobContainer = config["AzureStorage:BlobContainer"];
            CustomersTable = config["AzureStorage:TableName_Customers"];
            ProductsTable = config["AzureStorage:TableName_Products"];
            OrdersQueue = config["AzureStorage:QueueName_Orders"];
            FileShare = config["AzureStorage:FileShareName"];
        }
    }
}

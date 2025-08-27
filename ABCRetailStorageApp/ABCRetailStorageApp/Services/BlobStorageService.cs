using Azure.Storage.Blobs;

namespace ABCRetailStorageApp.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(string? connectionString = null)
        {
            connectionString ??= "DefaultEndpointsProtocol=https;AccountName=st10446952storage;AccountKey=CzispX7cxkdyVi0pHfZzh0IHox/KzeRvMvhAePBzROQPrIEk4JaO+rsDFIcWQayJGl5ZxJLUUMFW+AStEgMXPQ==;EndpointSuffix=core.windows.net";
            _containerClient = new BlobContainerClient(connectionString, "product-media");
            _containerClient.CreateIfNotExists();
        }

        public async Task<List<string>> GetBlobsAsync()
        {
            var list = new List<string>();
            await foreach (var blob in _containerClient.GetBlobsAsync()) list.Add(blob.Name);
            return list;
        }

        public async Task<string> UploadBlobAsync(IFormFile file)
        {
            var client = _containerClient.GetBlobClient(file.FileName);
            using var stream = file.OpenReadStream();
            await client.UploadAsync(stream, overwrite: true);
            return client.Uri.ToString();
        }

        public async Task DeleteBlobAsync(string name) => await _containerClient.DeleteBlobIfExistsAsync(name);
    }
}

using Azure.Storage.Files.Shares;
using Microsoft.AspNetCore.Http;

namespace ABCRetailStorageApp.Services
{
    public class FileStorageService
    {
        private readonly ShareClient _shareClient;

        public FileStorageService()
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=st10446952storage;AccountKey=CzispX7cxkdyVi0pHfZzh0IHox/KzeRvMvhAePBzROQPrIEk4JaO+rsDFIcWQayJGl5ZxJLUUMFW+AStEgMXPQ==;EndpointSuffix=core.windows.net";
            string shareName = "contracts";

            _shareClient = new ShareClient(connectionString, shareName);
            _shareClient.CreateIfNotExists();
        }

        public async Task<List<string>> GetFilesAsync()
        {
            var results = new List<string>();
            var directory = _shareClient.GetRootDirectoryClient();
            await foreach (var item in directory.GetFilesAndDirectoriesAsync())
                if (!item.IsDirectory) results.Add(item.Name);
            return results;
        }

        public async Task UploadFileAsync(IFormFile file)
        {
            var directory = _shareClient.GetRootDirectoryClient();
            var fileClient = directory.GetFileClient(file.FileName);
            using var stream = file.OpenReadStream();
            await fileClient.CreateAsync(stream.Length);
            await fileClient.UploadAsync(stream);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var directory = _shareClient.GetRootDirectoryClient();
            var fileClient = directory.GetFileClient(fileName);
            await fileClient.DeleteIfExistsAsync();
        }
    }
}

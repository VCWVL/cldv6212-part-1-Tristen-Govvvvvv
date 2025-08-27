using Azure.Storage.Queues;

namespace ABCRetailStorageApp.Services
{
    public class QueueStorageService
    {
        private readonly QueueClient _queueClient;

        public QueueStorageService(string? connectionString = null)
        {
            connectionString ??= "DefaultEndpointsProtocol=https;AccountName=st10446952storage;AccountKey=CzispX7cxkdyVi0pHfZzh0IHox/KzeRvMvhAePBzROQPrIEk4JaO+rsDFIcWQayJGl5ZxJLUUMFW+AStEgMXPQ==;EndpointSuffix=core.windows.net";
            _queueClient = new QueueClient(connectionString, "orders-queue");
            _queueClient.CreateIfNotExists();
        }

        // Add message to queue
        public async Task EnqueueAsync(string message)
        {
            string encoded = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(message));
            await _queueClient.SendMessageAsync(encoded);
        }

        // Peek messages without deleting them
        public async Task<List<string>> PeekMessagesAsync()
        {
            var list = new List<string>();
            var messages = await _queueClient.PeekMessagesAsync(maxMessages: 32);

            foreach (var msg in messages.Value)
            {
                list.Add(msg.MessageText);
            }

            return list;
        }

        // Optional: method to delete messages only if needed
        public async Task DeleteMessageAsync(string messageId, string popReceipt)
        {
            await _queueClient.DeleteMessageAsync(messageId, popReceipt);
        }
    }
}

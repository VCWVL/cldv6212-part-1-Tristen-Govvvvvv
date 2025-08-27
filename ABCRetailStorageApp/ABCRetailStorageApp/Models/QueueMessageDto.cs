namespace ABCRetailStorageApp.Models
{
    public class QueueMessageDto
    {
        public string Action { get; set; } // e.g., "ProcessOrder", "UploadImage"
        public string Payload { get; set; } // e.g., "OrderId:123" or "imageName.jpg"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ABCRetailStorageApp.Models
{
    public class OrderEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = "Order";
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        [Required]
        public string? CustomerRowKey { get; set; }  // make nullable

        [BindNever]
        public string? CustomerName { get; set; } = null;  // nullable and default null

        [Required]
        public string? ProductRowKey { get; set; }  // make nullable

        [BindNever]
        public string? ProductName { get; set; } = null;  // nullable and default null

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [BindNever]
        public double UnitPrice { get; set; } = 0;  // default 0

        [BindNever]
        public double TotalPrice { get; set; } = 0;  // default 0
    }
}

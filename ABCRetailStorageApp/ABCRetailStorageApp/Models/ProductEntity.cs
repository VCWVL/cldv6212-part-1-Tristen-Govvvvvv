using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ABCRetailStorageApp.Models
{
    public class ProductEntity : ITableEntity
    {
        public string PartitionKey { get; set; } = "Product";
        public string RowKey { get; set; } = Guid.NewGuid().ToString();
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        [Required]
        public string? Name { get; set; }  // make nullable

        [Required]
        [Range(0.01, double.MaxValue)]
        public double Price { get; set; }

        [BindNever]
        public string? ImageUrl { get; set; } = null;  // nullable and default null
    }
}

using Azure.Data.Tables;
using Azure;
using System.ComponentModel.DataAnnotations;

namespace ABC_Retail.Models
{
    public class Product : ITableEntity
    {
        [Key]
        public int Product_Id { get; set; }
        public string? ProductName { get; set; }
        public int? Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        // ITableEntity implementation
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}

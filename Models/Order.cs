using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ABC_Retail.Models
{
    public class Order : ITableEntity
    {
        public int? Order_Id { get; set; }

        [Required(ErrorMessage = "Please select a product.")]
        public int Product_Id { get; set; } 

        [Required(ErrorMessage = "Please select a customer.")]
        public int Customer_Id { get; set; }
        public string? Shipping_Address { get; set; }
        public DateTime Date {  get; set; }

        // ITableEntity implementation
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}

using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;

namespace ABC_Retail.Models
{
    public class Customer : ITableEntity
    {
        [Key]
        public int Customer_Id { get; set; }
        public string? Customer_Name { get; set;}
        public string? Customer_Email { get; set;}
        public string? Customer_Phone { get; set;}
        public string? Customer_Address { get; set;}

        // ITableEntity implementation
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }

    }
}

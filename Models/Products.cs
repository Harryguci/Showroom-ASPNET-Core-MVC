using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ShowroomManagement.Models
{
    public class Products
    {
        [Key]
        public string Serial { get; set; }
        public string? Name { get; set; }
        public int? Value { get; set; }
        public string? InEnterId { get; set; }
        public string? InSaleId { get; set; }
        public string? Status { get; set; }

        public Products(string serial)
        {
            Serial = serial;
        }
    }
}

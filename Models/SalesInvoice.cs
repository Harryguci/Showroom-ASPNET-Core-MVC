using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShowroomManagement.Models
{
    public class SalesInvoice
    {
        public SalesInvoice() {
            InSalesId = ClientId = string.Empty;
        }
        [Key]
        public string InSalesId { get; set; }
        public string ClientId { get; set; }
        [Column("Date")]
        public DateTime SaleDate { get; set; }
        public string? Status { get; set; }
        public int? QuantitySale { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShowroomManagement.Models
{
    public class PurchaseInvoice
    {
        public PurchaseInvoice() {
            InEnterId = Source = string.Empty;
            EnteredDate = DateTime.Now;
        }
        [Key]
        public string InEnterId { get; set; }
        public string Source { get; set; }
        [Column("Date")]
        public DateTime EnteredDate { get; set; }
        public int? QuantityPurchase { get; set; }
        public string? Status { get; set; }
    }
}

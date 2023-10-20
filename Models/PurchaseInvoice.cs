using System.ComponentModel.DataAnnotations;

namespace ShowroomManagement.Models
{
    public class PurchaseInvoice
    {
        public PurchaseInvoice() {
            InEnterId = Source = string.Empty;
            Price = 0;
            EnteredDate = DateTime.Now;
        }
        [Key]
        public string InEnterId { get; set; }
        public string Source { get; set; }
        public int Price { get; set; }
        public DateTime EnteredDate { get; set; }

        public string? Status { get; set; }
        public string? EmployeeId { get; set; }

        public List<Employee>? Employees { get; set; }
    }
}

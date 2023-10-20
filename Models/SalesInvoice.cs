using System.ComponentModel.DataAnnotations;

namespace ShowroomManagement.Models
{
    public class SalesInvoice
    {
        public SalesInvoice() {
            InSalesId = ClientId = string.Empty;
            Price = 0;
        }
        [Key]
        public string InSalesId { get; set; }
        public string ClientId { get; set; }
        public int Price { get; set; }
        public DateTime SaleDate { get; set; }
        public string? Status { get; set; }
        public string? EmployeeId { get; set; }
        public List<Employee>? Employees { get; set; }
    }
}

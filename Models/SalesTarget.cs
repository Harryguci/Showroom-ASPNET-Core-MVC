using System.ComponentModel.DataAnnotations;

namespace ShowroomManagement.Models
{
    public class SalesTarget
    {
        public SalesTarget(string saleId)
        {
            SaleId = saleId;
        }
        [Key]
        public string SaleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Total { get; set; }
        public int Target { get; set; }
        public string? Status { get; set; }
        public int Rate { get; set; }
    }
}

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

        [Display(Name = "ID")]
        public string SaleId { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Ngày kết thúc")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Tổng")]
        public int Total { get; set; }

        [Display(Name = "Mục tiêu")]
        public int Target { get; set; }
        
        [Display(Name = "Trạng thái")]
        public string? Status { get; set; }

        [Display(Name = "Tổng thưởng")]
        public float? Reward { get; set; }
    }
}

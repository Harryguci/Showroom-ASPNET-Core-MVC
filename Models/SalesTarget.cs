using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShowroomManagement.Models
{
    public class SalesTarget
    {
        public SalesTarget()
        {
            SaleId = string.Empty;
        }
        [Key]

        [Display(Name = "ID")]
        [JsonPropertyName("SaleId")]
        public string SaleId { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        [JsonPropertyName("StartDate")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Ngày kết thúc")]
        [JsonPropertyName("EndDate")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Tổng")]
        [JsonPropertyName("Total")]
        public int Total { get; set; }

        [Display(Name = "Mục tiêu")]
        [JsonPropertyName("Target")]
        public int Target { get; set; }
        
        [Display(Name = "Trạng thái")]
        [JsonPropertyName("Status")]
        public string Status { get; set; }

        [Display(Name = "Tổng thưởng")]
        [JsonPropertyName("Reward")]
        public double Reward { get; set; }

        [Display(Name = "Mã nhân viên")]
        [JsonPropertyName("EmployeeId")]
        [Column("EmployeeId")]
        public string EmployeeId { get; set; }
    }
}

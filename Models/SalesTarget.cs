using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("saleId")]
        public string SaleId { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Ngày kết thúc")]
        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Tổng")]
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [Display(Name = "Mục tiêu")]
        [JsonPropertyName("target")]
        public int Target { get; set; }
        
        [Display(Name = "Trạng thái")]
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [Display(Name = "Tổng thưởng")]
        [JsonPropertyName("reward")]
        public float? Reward { get; set; }
    }
}

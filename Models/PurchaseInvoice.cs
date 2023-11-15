using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShowroomManagement.Models
{
    public class PurchaseInvoice
    {
        public PurchaseInvoice() {
            InEnterId = SourceId = string.Empty;
            EnteredDate = DateTime.Now;
        }

        [Key]
        [Display(Name = "ID")]
        [JsonPropertyName("InEnterId")]
        public string InEnterId { get; set; }

        [Display(Name = "Nhà cung cấp")]
        [JsonPropertyName("SourceId")]
        public string SourceId { get; set; }

        [Display(Name = "Mã sản phẩm")]
        [JsonPropertyName("ProductId")]
        public string ProductId { get; set; }

        [Column("Date")]
        [Display(Name = "Ngày nhập")]
        [JsonPropertyName("EnteredDate")]
        public DateTime EnteredDate { get; set; }

        [Display(Name = "Số lượng")]
        [JsonPropertyName("QuantityPurchase")]
        public int QuantityPurchase { get; set; }

        [Display(Name = "Trạng thái")]
        [JsonPropertyName("Status")]
        public string Status { get; set; }
    }
}

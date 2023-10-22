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
        [JsonPropertyName("inEnterId")]
        public string InEnterId { get; set; }

        [Display(Name = "Nhà cung cấp")]
        [JsonPropertyName("sourceId")]
        public string SourceId { get; set; }

        [Column("Date")]
        [Display(Name = "Ngày nhập")]
        [JsonPropertyName("enteredDate")]
        public DateTime EnteredDate { get; set; }

        [Display(Name = "Số lượng")]
        [JsonPropertyName("quantityPurchase")]
        public int? QuantityPurchase { get; set; }

        [Display(Name = "Trạng thái")]
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}

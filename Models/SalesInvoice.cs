using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShowroomManagement.Models
{
    public class SalesInvoice
    {
        public SalesInvoice()
        {
            InSaleId = ClientId = ProductId = string.Empty;
        }

        [Key]
        [Display(Name = "Mã hóa đơn")]
        [JsonPropertyName("InSaleId")]
        public string InSaleId { get; set; }

        [Display(Name = "Mã khách hàng")]
        [JsonPropertyName("ClientId")]
        public string ClientId { get; set; }

        [Display(Name = "Mã sản phẩm")]
        [JsonPropertyName("ProductId")]
        public string ProductId { get; set; }

        [Column("Date")]
        [Display(Name = "Ngày bán")]
        [JsonPropertyName("SaleDate")]
        public DateTime SaleDate { get; set; }

        [Display(Name = "Trạng thái")]
        [JsonPropertyName("Status")]
        public string Status { get; set; }

        [Display(Name = "Số lượng")]
        [JsonPropertyName("QuantitySale")]
        public int QuantitySale { get; set; }
    }
}

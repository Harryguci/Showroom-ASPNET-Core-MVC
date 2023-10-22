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
        [JsonPropertyName("inSaleId")]
        public string InSaleId { get; set; }

        [Display(Name = "Mã khách hàng")]
        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [Display(Name = "Mã sản phẩm")]
        [JsonPropertyName("productId")]
        public string ProductId { get; set; }

        [Column("Date")]
        [Display(Name = "Ngày bán")]
        [JsonPropertyName("saleDate")]
        public DateTime SaleDate { get; set; }

        [Display(Name = "Trạng thái")]
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [Display(Name = "Số lượng")]
        [JsonPropertyName("quantitySale")]
        public int? QuantitySale { get; set; }
    }
}

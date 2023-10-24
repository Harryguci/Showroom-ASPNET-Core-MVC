using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ShowroomManagement.Models
{
    public class Products
    {
        [Key]
        [Display(Name = "Số seri")]
        [JsonPropertyName("serial")]
        public string Serial { get; set; }

        [Display(Name = "Tên")]
        [JsonPropertyName("productName")]
        public string ProductName { get; set; }

        [Display(Name = "Giá nhập")]
        [JsonPropertyName("purchasePrice")]
        public int? PurchasePrice { get; set; }
        
        [Display(Name = "Giá bán")]
        [JsonPropertyName("salePrice")]
        public int? SalePrice { get; set; }
        
        [Display(Name = "Số lượng")]
        [JsonPropertyName("quantity")]
        public int? Quantity { get; set; }
        
        [Display(Name = "Trạng thái")]
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        public Products()
        {
            Serial = string.Empty;
            ProductName = string.Empty;
        }
    }   
}

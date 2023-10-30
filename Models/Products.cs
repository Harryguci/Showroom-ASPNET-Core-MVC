using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ShowroomManagement.Models
{
    public class Products
    {
        [Key]
        [Display(Name = "Số seri")]
        [JsonPropertyName("Serial")]
        public string Serial { get; set; }

        [Display(Name = "Tên")]
        [JsonPropertyName("ProductName")]
        public string ProductName { get; set; }

        [Display(Name = "Giá nhập")]
        [JsonPropertyName("PurchasePrice")]
        public int? PurchasePrice { get; set; }
        
        [Display(Name = "Giá bán")]
        [JsonPropertyName("SalePrice")]
        public int? SalePrice { get; set; }
        
        [Display(Name = "Số lượng")]
        [JsonPropertyName("Quantity")]
        public int? Quantity { get; set; }
        
        [Display(Name = "Trạng thái")]
        [JsonPropertyName("Status")]
        public string Status { get; set; }

        [Column("Deleted")]
        [JsonPropertyName("Deleted")]
        public bool? Deleted { get; set; } = false;

        public List<ProductImages> ImageUrls { get; set; }

        public Products()
        {
            Serial = string.Empty;
            ProductName = string.Empty;
        }
    }   
}

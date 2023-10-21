using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ShowroomManagement.Models
{
    public class Products
    {
        [Key]
        [Display(Name = "Số seri")]
        public string Serial { get; set; }

        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Display(Name = "Giá nhập")]
        public int? PurchasePrice { get; set; }
        
        [Display(Name = "Giá bán")]
        public int? SalePrice { get; set; }
        
        [Display(Name = "Số lượng")]
        public int? Quantity { get; set; }
        
        [Display(Name = "Nhà cung cấp")]
        public string? SourceId { get; set; }
        
        [Display(Name = "Trạng thái")]
        public string? Status { get; set; }

        public List<Source>? Sources { get; set; }

        public Products()
        {
            Serial = string.Empty;
            Name = string.Empty;
        }
    }
}

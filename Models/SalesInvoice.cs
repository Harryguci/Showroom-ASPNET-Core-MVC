using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShowroomManagement.Models
{
    public class SalesInvoice
    {
        public SalesInvoice() {
            InSalesId = ClientId = string.Empty;
        }

        [Key]
        [Display(Name = "Mã hóa đơn")]
        public string InSalesId { get; set; }

        [Display(Name = "Mã khách hàng")]
        public string ClientId { get; set; }

        [Column("Date")]
        [Display(Name = "Ngày bán")]
        public DateTime SaleDate { get; set; }

        [Display(Name = "Trạng thái")]
        public string? Status { get; set; }

        [Display(Name = "Số lượng")]
        public int? QuantitySale { get; set; }
    }
}

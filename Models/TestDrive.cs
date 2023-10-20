using System.ComponentModel.DataAnnotations;

namespace ShowroomManagement.Models
{
    public class TestDrive
    {
        public TestDrive() {

            DriveId = ClientId = string.Empty;
        }
        [Key]
        [Display(Name = "ID")]
        public string DriveId { get; set; }
        [Display(Name = "Mã khách hàng")]
        public string ClientId{ get; set; }
        [Display(Name = "Ngày đặt")]
        public DateTime BookDate { get; set; }
        [Display(Name = "Ghi chú")]
        public string? Note { get; set; }
        [Display(Name = "Trạng thái")]
        public string? Status { get; set; }
    }
}

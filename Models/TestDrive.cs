using System.ComponentModel.DataAnnotations;

namespace ShowroomManagement.Models
{
    public class TestDrive
    {
        public TestDrive() {

            DriveId = ClientId = EmployeeId = string.Empty;
        }

        [Key]
        [Display(Name = "ID")]
        [StringLength(maximumLength: 10)]
        [Required(ErrorMessage = "Bạn phải nhập Id")]
        public string DriveId { get; set; }

        [Display(Name = "Mã khách hàng")]
        [StringLength(maximumLength: 10)]
        [Required(ErrorMessage = "Bạn phải nhập mã khách hàng")]
        public string ClientId{ get; set; }

        [Display(Name = "Mã nhân viên")]
        [StringLength(maximumLength: 10)]
        public string EmployeeId { get; set; }

        [Display(Name = "Ngày đặt")]
        [Required(ErrorMessage = "Bạn phải chọn ngày đặt")]
        public DateTime BookDate { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(maximumLength: 200)]
        public string Note { get; set; }

        [Display(Name = "Trạng thái")]
        public string Status { get; set; }
    }
}

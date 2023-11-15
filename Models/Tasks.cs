using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ShowroomManagement.Models
{
    public class Tasks
    {
        [Key]
        [StringLength(10, ErrorMessage = "Id quá dài")]
        public string Id { get; set; }

        [StringLength(10, ErrorMessage = "Mã nhân viên quá dài")]
        [Required(ErrorMessage = "Bạn phải chỉ định 1 nhân viên")]
        public string EmployeeId { get; set; }

        [StringLength(500, ErrorMessage = "Nội dung quá dài")]
        [Required(ErrorMessage = "Bạn phải điền nội dung")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Bạn phải chọn ngày đến hạn")]
        public DateTime Dateline { get; set; }

        [AllowNull]
        [StringLength(maximumLength: 500)]
        public string Result { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ShowroomManagement.Models
{
    public class Tasks
    {
        [Key]
        [StringLength(10)]
        public string Id { get; set; }
        [StringLength(10)]
        public string EmployeeId { get; set; }
        [StringLength(500)]
        public string Content { get; set; }
        [Required(ErrorMessage = "Bạn phải chọn ngày đến hạn")]
        public DateTime Dateline { get; set; }
        public string Result { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ShowroomManagement.Models
{
    public class Employee
    {
        public Employee()
        {
            EmployeeId = Firstname = Lastname = Cccd = Position = string.Empty;
            Salary = 0;
        }
        [Key]
        [Display(Name = "ID")]
        [StringLength(maximumLength: 10)]
        public string EmployeeId { get; set; }

        [Display(Name = "Họ")]
        [StringLength(maximumLength: 190)]
        public string? Firstname { get; set; }

        [Display(Name = "Tên")]
        [StringLength(maximumLength: 190)]
        public string? Lastname { get; set; }

        [Display(Name = "Ngày sinh")]
        public DateTime? DateBirth { get; set; }

        [Display(Name = "CCCD")]
        public string? Cccd { get; set; }

        [Display(Name = "Vị trí")]
        public string? Position { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Lương")]
        public int Salary { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Doanh số")]
        public string? SaleId { get; set; }

        [Display(Name = "Giới tính")]
        public bool? Gender { get; set; }
    }
}

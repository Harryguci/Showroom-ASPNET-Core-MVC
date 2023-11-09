using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

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
        [JsonPropertyName("EmployeeId")]
        public string EmployeeId { get; set; }

        [Display(Name = "Họ")]
        [StringLength(maximumLength: 190)]
        [JsonPropertyName("Firstname")]
        public string Firstname { get; set; }

        [Display(Name = "Tên")]
        [JsonPropertyName("Lastname")]
        [StringLength(maximumLength: 190)]
        public string Lastname { get; set; }

        [Display(Name = "Ngày sinh")]
        [JsonPropertyName("DateBirth")]
        public DateTime DateBirth { get; set; }

        [Display(Name = "CCCD")]
        [JsonPropertyName("Cccd")]
        public string Cccd { get; set; }

        [Display(Name = "Vị trí")]
        [JsonPropertyName("Position")]
        public string Position { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        [JsonPropertyName("StartDate")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Lương")]
        [JsonPropertyName("Salary")]
        public int Salary { get; set; }

        [Display(Name = "Email")]
        [JsonPropertyName("Email")]
        public string Email { get; set; }

        [Display(Name = "Giới tính")]
        [JsonPropertyName("Gender")]
        public bool Gender { get; set; }

        public bool Deleted { get; set; } = false;

        [Display(Name = "Ảnh")]
        [JsonPropertyName("Url_image")]
        [AllowNull]
        public string Url_image { get; set; } = string.Empty;
    }
}

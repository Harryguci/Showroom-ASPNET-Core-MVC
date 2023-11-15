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

        [Required(ErrorMessage = "Bạn phải nhập họ")]
        [Display(Name = "Họ")]
        [StringLength(maximumLength: 190, ErrorMessage = "Họ quá dài")]
        [JsonPropertyName("Firstname")]
        public string Firstname { get; set; }

        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Bạn phải nhập tên")]
        [JsonPropertyName("Lastname")]
        [StringLength(maximumLength: 190, ErrorMessage = "Tên quá dài")]
        public string Lastname { get; set; }

        [Display(Name = "Ngày sinh")]
        [JsonPropertyName("DateBirth")]
        public DateTime DateBirth { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập CCCD")]
        [StringLength(maximumLength: 12, MinimumLength = 9, ErrorMessage = "CCCD phải từ 9 đến 12 chữ số")]
        [Display(Name = "CCCD")]
        [JsonPropertyName("Cccd")]
        public string Cccd { get; set; }

        [Display(Name = "Vị trí")]
        [JsonPropertyName("Position")]
        [Required(ErrorMessage = "Bạn phải nhập vị trí")]
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

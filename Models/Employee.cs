using System.ComponentModel.DataAnnotations;
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
        [JsonPropertyName("employeeId")]
        public string EmployeeId { get; set; }

        [Display(Name = "Họ")]
        [StringLength(maximumLength: 190)]
        [JsonPropertyName("firstname")]
        public string Firstname { get; set; }

        [Display(Name = "Tên")]
        [JsonPropertyName("lastname")]
        [StringLength(maximumLength: 190)]
        public string Lastname { get; set; }

        [Display(Name = "Ngày sinh")]
        [JsonPropertyName("datebirth")]
        public DateTime? DateBirth { get; set; }

        [Display(Name = "CCCD")]
        [JsonPropertyName("cccd")]
        public string Cccd { get; set; }

        [Display(Name = "Vị trí")]
        public string Position { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        [JsonPropertyName("startdate")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Lương")]
        [JsonPropertyName("salary")]
        public int Salary { get; set; }

        [Display(Name = "Email")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Display(Name = "Doanh số")]
        [JsonPropertyName("saleId")]
        public string SaleId { get; set; }

        [Display(Name = "Giới tính")]
        [JsonPropertyName("gender")]
        public bool Gender { get; set; }

        public bool Deleted { get; set; } = false;

        [Display(Name = "Ảnh")]
        [JsonPropertyName("url_image")]
        public string Url_image { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ShowroomManagement.Models
{
    public class Customer
    {
        public Customer() {
            ClientId = string.Empty;
            Firstname = string.Empty;
            Lastname = string.Empty;
        }

        [Key]
        [Display(Name = "ID")]
        [JsonPropertyName("ClientId")]
        public string ClientId { get; set; }

        [Display(Name = "Họ")]
        [JsonPropertyName("Firstname")]
        [Required(ErrorMessage = "Bạn phải nhập họ")]
        public string Firstname { get; set; }

        [JsonPropertyName("Lastname")]
        [Required(ErrorMessage = "Bạn phải nhập tên")]
        [Display(Name = "Tên")]
        public string Lastname { get; set; }

        [Display(Name = "Ngày sinh")]
        [JsonPropertyName("DateBirth")]
        public DateTime DateBirth { get; set; }
        
        [Display(Name = "CCCD")]
        [StringLength(maximumLength: 12, MinimumLength = 9, ErrorMessage = "CCCD phải tối thiểu 9 và tối đa 12 chữ số.")]
        [JsonPropertyName("Cccd")]
        public string Cccd { get; set; }

        [Display(Name = "Email")]
        [JsonPropertyName("Email")]
        public string Email { get; set; }

        [StringLength(maximumLength: 200, ErrorMessage = "Địa chỉ quá dài")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Giới tính")]
        [JsonPropertyName("Gender")]
        public bool Gender { get; set; }

        [JsonPropertyName("Deleted")]
        public bool Deleted { get; set; } = false;
    }
}

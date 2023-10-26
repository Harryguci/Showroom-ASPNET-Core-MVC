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
        public string Firstname { get; set; }

        [JsonPropertyName("Lastname")]
        [Display(Name = "Tên")]
        public string Lastname { get; set; }

        [Display(Name = "Ngày sinh")]
        [JsonPropertyName("DateBirth")]
        public DateTime DateBirth { get; set; }
        
        [Display(Name = "CCCD")]
        [JsonPropertyName("Cccd")]
        public string Cccd { get; set; }

        [Display(Name = "Email")]
        [JsonPropertyName("Email")]
        public string Email { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Giới tính")]
        [JsonPropertyName("Gender")]
        public bool Gender { get; set; }

        [JsonPropertyName("Deleted")]
        public bool Deleted { get; set; } = false;
    }
}

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
        [JsonPropertyName("clientId")]
        public string ClientId { get; set; }

        [Display(Name = "Họ")]
        [JsonPropertyName("firstname")]
        public string Firstname { get; set; }

        [JsonPropertyName("lastname")]
        [Display(Name = "Tên")]
        public string Lastname { get; set; }

        [Display(Name = "Ngày sinh")]
        [JsonPropertyName("datebirth")]
        public DateTime? DateBirth { get; set; }
        
        [Display(Name = "CCCD")]
        [JsonPropertyName("cccd")]
        public string? Cccd { get; set; }

        [Display(Name = "Email")]
        [JsonPropertyName("email")]
        public string? Email { get; set; }
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }

        [Display(Name = "Giới tính")]
        [JsonPropertyName("gender")]
        public bool Gender { get; set; }

        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; } = false;
    }
}

using System.ComponentModel.DataAnnotations;

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
        public string ClientId { get; set; }
        [Display(Name = "Họ")]
        public string Firstname { get; set; }
        [Display(Name = "Tên")]
        public string Lastname { get; set; }
        [Display(Name = "Ngày sinh")]
        public DateTime? DateBirth { get; set; }
        [Display(Name = "CCCD")]
        public string? Cccd { get; set; }
        [Display(Name = "Email")]
        public string? Email { get; set; }
        [Display(Name = "Địa chỉ")]
        public string? Address { get; set; }
    }
}

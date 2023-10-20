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
        public string ClientId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? DateBirth { get; set; }
        public string? Cccd { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}

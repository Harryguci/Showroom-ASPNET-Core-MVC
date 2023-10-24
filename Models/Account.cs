using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ShowroomManagement.Models
{
    public class Account
    {
        public Account()
        {
            Username = Password = string.Empty;    
        }

        [Key]
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [Column(name: "Password_foruser", TypeName = "varbinary(500)")]
        [JsonPropertyName("password")]
        
        public string Password { get; set; }

        public string? EmployeeId { get; set; }

        [Column("Level_account")]
        public int? Level { get; set; } = 0;

        public bool? Deleted { get; set; } = false;

        public DateTime? CreateAt { get; set; }
        public DateTime? DeleteAt { get; set; }
    }
}

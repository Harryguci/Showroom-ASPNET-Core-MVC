using System.ComponentModel.DataAnnotations;

namespace ShowroomManagement.Models
{
    public class TestDrive
    {
        public TestDrive() {

            DriveId = ClientId = string.Empty;
        }
        [Key]
        public string DriveId { get; set; }
        public string ClientId{ get; set; }
        public DateTime BookDate { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
    }
}

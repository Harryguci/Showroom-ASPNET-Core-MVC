using Microsoft.CodeAnalysis.Operations;
using System.ComponentModel.DataAnnotations;

namespace ShowroomManagement.Models
{
    public class Device
    {
        public Device()
        {
            DeviceId = string.Empty;
            Serial = string.Empty;
        }
        [Key]
        public string DeviceId { get; set; }
        public string Serial { get; set; }
        public DateTime? DateEntry { get; set; }
        public string? Status { get; set; }
        public int? Price { get; set; }
    }
}

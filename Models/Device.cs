using Microsoft.CodeAnalysis.Operations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShowroomManagement.Models
{
    public class Device
    {
        public Device(string deviceId, string deviceName)
        {
            DeviceId = deviceId;
            DeviceName = deviceName;
        }
        [Key]
        [Display(Name = "Mã")]
        public string DeviceId { get; set; }

        [Column("Name")]
        [Display(Name = "Tên")]
        public string DeviceName { get; set; }
        
        [Display(Name = "Ngày nhập")]
        public DateTime? DateEntry { get; set; }
        
        [Display(Name = "Trạng thái")]
        public string? Status { get; set; }
        
        [Display(Name = "Giá trị")]
        public int? Price { get; set; }
    }
}

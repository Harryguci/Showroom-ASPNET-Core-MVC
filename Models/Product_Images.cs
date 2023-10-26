using System.ComponentModel.DataAnnotations;

namespace ShowroomManagement.Models
{
    public class Product_Images
    {
        [Key]
        public int Id { get; set; }
        public string ProductSerial { get; set; }
        public string Url_image { get; set; }
    }
}

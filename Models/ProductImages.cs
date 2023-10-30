using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShowroomManagement.Models
{
    public class ProductImages
    {
        [Key]
        public int Id { get; set; }
        public string Serial { get; set; }
        public string Url_image { get; set; }
    }
}

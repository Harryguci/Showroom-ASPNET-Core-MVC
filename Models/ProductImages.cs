using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace ShowroomManagement.Models
{
    public class ProductImages
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public string Serial { get; set; }
        public string Url_image { get; set; }
    }
}

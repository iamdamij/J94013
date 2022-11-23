using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace J94013.Models
{
    public class Menu
    {

        [Key]
        public int ID { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name { get; set; } = string.Empty;
        [StringLength(500, MinimumLength = 3)]
        [Required]
        public string Cartegory { get; set; } = string.Empty;
        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        public string Description { get; set; } = string.Empty;
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        [Display(Name = " ")]
        public string ImgImageData { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}

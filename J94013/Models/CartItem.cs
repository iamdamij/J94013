using System.ComponentModel.DataAnnotations;

namespace J94013.Models
{
    public class CartItem
    {
        [Required]
        public int MenuID { get; set; }
        [Required]
        public int CartID { get; set; }
        [Required]
        public int Quanity { get; set; }
    }
}

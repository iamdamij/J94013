using System.ComponentModel.DataAnnotations;

namespace J94013.Models
{
    public class CheckoutItem
    {
        [Key, Required]
        public int ID { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Name { get; set; }
        [Required ]
        public int Quantity { get; set; }
    }
}

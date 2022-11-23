using System.ComponentModel.DataAnnotations;

namespace J94013.Models
{
    public class CheckoutCustomer
    {
        [Key, StringLength(100)]
        public string Email { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public int CartID { get; set; }

    }
}

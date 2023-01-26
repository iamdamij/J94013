using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace J94013.Models
{
    public class CheckoutCustomers
    {
        [Key, StringLength(100)]
        public string Email { get; set; }
        public string? Name { get; set; }
        public int CartID { get; set; }

    }
}

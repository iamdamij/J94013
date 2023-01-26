using System.ComponentModel.DataAnnotations;

namespace J94013.Models
{
    public class OrderHistory
    {
        [Key, Required]
        public int OrderNo { get; set; }
        [Required]
        public string Email { get; set; }

    }
}

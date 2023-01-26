using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace J94013.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }
        public string Name { get; set; }
        public string  PhoneNo { get; set; }
        public string Email { get; set; }
        [Display(Name = "Booking Date")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }
    }
}

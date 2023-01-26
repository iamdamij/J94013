using J94013.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace J94013.Pages.Admin
{
    [Authorize (Roles = "Admin")]
    public class BookingModel : PageModel
    {
        private readonly J94013.Data1.J94013DbContext _context;

        public BookingModel(J94013.Data1.J94013DbContext context)
        {
            _context = context;
        }

        public IList<Booking> Bookings { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        public SelectList? Email { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? BookingEmail { get; set; }
        public async Task OnGetAsync()
        {
            IQueryable<string> emailQuery = from m in _context.Bookings
                                                orderby m.Email
                                                select m.Email;
            var bookings = from m in _context.Bookings
                        select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                bookings = bookings.Where(s => s.Name.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(BookingEmail))
            {
                bookings = bookings.Where(x => x.Email == BookingEmail);
            }
            Email = new SelectList(await emailQuery.Distinct().ToListAsync());
            Bookings = await bookings.ToListAsync();
        }
    }
}
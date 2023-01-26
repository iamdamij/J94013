using J94013.Data1;
using J94013.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace J94013.Pages
{
    
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly J94013.Data1.J94013DbContext _context;

        [BindProperty]
        public Booking Booking { get; set; }

        public IndexModel(ILogger<IndexModel> logger, J94013DbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult OnGet()
        {
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            
            _context.Bookings.Add(Booking);
            _context.SaveChanges();


            return RedirectToPage("./Index");
        }

    }
}
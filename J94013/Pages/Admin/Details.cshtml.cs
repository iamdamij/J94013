using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using J94013.Data1;
using J94013.Models;
using Microsoft.AspNetCore.Authorization;

namespace J94013.Pages.Menus
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly J94013.Data1.J94013DbContext _context;

        public DetailsModel(J94013.Data1.J94013DbContext context)
        {
            _context = context;
        }

      public Menu Menu { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menu.FirstOrDefaultAsync(m => m.ID == id);
            if (menu == null)
            {
                return NotFound();
            }
            else 
            {
                Menu = menu;
            }
            return Page();
        }
    }
}

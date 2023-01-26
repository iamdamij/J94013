using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using J94013.Data1;
using J94013.Models;
using Microsoft.AspNetCore.Authorization;

namespace J94013.Pages.Menus
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly J94013.Data1.J94013DbContext _context;

        public IndexModel(J94013.Data1.J94013DbContext context)
        {
            _context = context;
        }

        public IList<Menu> Menu { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }
        public SelectList? Cartegory { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? MenuCartegory { get; set; }
        public async Task OnGetAsync()
        {
            IQueryable<string> cartegoryQuery = from m in _context.Menu
                                            orderby m.Cartegory
                                            select m.Cartegory;
            var menus = from m in _context.Menu
                        select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                menus = menus.Where(s => s.Name.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(MenuCartegory))
            {
                menus = menus.Where(x => x.Cartegory == MenuCartegory);
            }
            Cartegory = new SelectList(await cartegoryQuery.Distinct().ToListAsync());
            Menu = await menus.ToListAsync();
        }
    }
}
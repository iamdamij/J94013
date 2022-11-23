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
using Microsoft.AspNetCore.Identity;

namespace J94013.Pages.Menus
{
    public class MenuModel : PageModel
    {
        private readonly J94013.Data1.J94013DbContext _context;
        private readonly UserManager<ApplicationIdentity> um;
        private readonly UserManager<ApplicationIdentity> _userManager;
        public MenuModel(J94013.Data1.J94013DbContext context, UserManager<ApplicationIdentity> um)
        {
            _context = context;
            _userManager = um;
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
        public async Task<IActionResult> OnPostAddtocartAsync(int itemID)
        {
            var user = await _userManager.GetUserAsync(User);
            CheckoutCustomer customer = await _context.CheckoutCustomers.FindAsync(user.FullName);
            var item = _context.CartItems.FromSqlRaw("SELECT * FROM CartItems WHERE MenuID ={0} AND CartID = {1}", itemID, customer.CartID)
            .ToList().FirstOrDefault();

            if (item == null)
            {
                CartItem newItem = new CartItem
                {
                    CartID = customer.CartID,
                    MenuID = itemID,
                    Quanity = 1
                };
                _context.CartItems.Add(newItem);
                await _context.SaveChangesAsync();
            }
            else
            {
                item.Quanity += 1;
                _context.Attach(item).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException e)
                {
                    throw new Exception($"Unamle to add item to cart", e);
                }
            }
            return RedirectToPage();
        }
    }
}


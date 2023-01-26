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
using Microsoft.AspNetCore.Authorization;

namespace J94013.Pages
{
    public class MenusModel : PageModel
    {
        
        private readonly J94013.Data1.J94013DbContext _context;

        private readonly UserManager<IdentityUser> _userManager;
        public MenusModel(J94013.Data1.J94013DbContext context, UserManager<IdentityUser> um)
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

        

       

       
        //public IActionResult OnPostSearch()
        //{
        //    Menu = _context.Menu.FromSqlRaw("SELECT * FROM Menu WHERE Name LIKE '" + SearchString + "%'").ToList();
        //    return Page();
        //}

       public async Task OnGetAsync()
        {
            //Menu = _context.Menu.FromSqlRaw("SELECT * FROM Menu WHERE Active = 1").ToList();
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
        
        public async Task<IActionResult> OnPostBuyAsync(int itemID)
        {
           
            var user = await _userManager.GetUserAsync(User);
            CheckoutCustomers customer = await _context.CheckoutCustomers.FindAsync(user.Email);
            var item = _context.CartItems.FromSqlRaw("SELECT * FROM CartItems WHERE MenuID ={0} AND CartID = {1}", itemID, customer.CartID)
            .ToList().FirstOrDefault();

            if (item == null)
            {
                CartItem newItem = new CartItem
                {
                    CartID = customer.CartID,
                    MenuID = itemID,
                    Quantity = 1
                };
                _context.CartItems.Add(newItem);
                await _context.SaveChangesAsync();
            }
            else
            {
                item.Quantity += 1;
                _context.Attach(item).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException e)
                {
                    throw new Exception($"Unable to add item to cart", e);
                }
            }
            return RedirectToPage();
        }
    }
}


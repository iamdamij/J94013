using J94013.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.FinancialConnections;

namespace J94013.Pages
{
    [Authorize]
    public class CheckoutModel : PageModel
    {
        public IList<CheckoutItem> Items { get; set; } = default!;

        private readonly J94013.Data1.J94013DbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public OrderHistory Order = new OrderHistory();
        
        public decimal OrderTotal = 0;
        public long AmountPayable = 0;

        public CheckoutModel(J94013.Data1.J94013DbContext context, UserManager<IdentityUser> um)
        {
            _context = context;
            _userManager = um;
        }
        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            CheckoutCustomers customers = await _context.CheckoutCustomers.FindAsync(user.Email);

            Items = _context.CheckoutItems.FromSqlRaw(
                "SELECT Menu.ID, Menu.Price, Menu.Name, " + 
                "CartItems.CartID, CartItems.Quantity " + 
                "FROM Menu INNER JOIN CartItems ON Menu.ID = CartItems.MenuID "
                + "WHERE CartID = {0}", customers.CartID).ToList();
            OrderTotal = 0;
            foreach (var item in Items)
            {
                OrderTotal += (item.Price) * (item.Quantity);
            }
            AmountPayable = (long)(OrderTotal * 100);
        }

        public async Task<IActionResult> OnPostBuyAsync()
        {
            var currentOrder = _context.OrderHistories.FromSqlRaw("SELECT * FROM OrderHistories").OrderByDescending(b => b.OrderNo).FirstOrDefault();
            if (currentOrder == null)
            {
                Order.OrderNo = 1;
            }
            else
            {
                Order.OrderNo = currentOrder.OrderNo + 1;
            }
            var user = await _userManager.GetUserAsync(User);
            Order.Email = user.Email;
            _context.OrderHistories.Add(Order);

            CheckoutCustomers customers = await _context.CheckoutCustomers.FindAsync(user.Email);

            var CartItems = _context.CartItems.FromSqlRaw("SELECT * FROM CartItems WHERE CartID = {0}", customers.CartID).ToList();
            foreach (var item in CartItems)
            {
                OrderItem oi = new OrderItem
                {
                    OrderNo = Order.OrderNo,
                    MenuID = item.MenuID,
                    Quantity = item.Quantity
                };
                _context.OrderItems.Add(oi);
                _context.CartItems.Remove(item);
                
            }
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
        public IActionResult OnPostCharge(string stripeEmail, string stripeToken, long amount)
        {
            var customers = new CustomerService();
            var charges = new ChargeService();

            var customer = customers.Create(new CustomerCreateOptions
            {
               Email = stripeEmail,
               Source = stripeToken
            });
            var charge = charges.Create(new ChargeCreateOptions
            {
                Amount = amount,
                Description = "CO5227 Restuarant Charges",
                Customer = customer.Id
            });

          
            return RedirectToPage("/Index");
        }
    }
}

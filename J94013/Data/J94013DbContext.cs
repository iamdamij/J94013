using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using J94013.Models;

namespace J94013.Data1
{
    public class J94013DbContext : DbContext
    {
        public J94013DbContext (DbContextOptions<J94013DbContext> options)
            : base(options)
        {
        }

        public DbSet<J94013.Models.Menu> Menu { get; set; }
        public DbSet<J94013.Models.CheckoutCustomer> CheckoutCustomers { get; set; }
        public DbSet<J94013.Models.Cart> Carts  { get; set; }
        public DbSet<J94013.Models.CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<CartItem>().HasKey(t => new { t.MenuID });
        }
    }
}

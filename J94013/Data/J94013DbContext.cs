using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using J94013.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace J94013.Data1
{
    public class J94013DbContext : DbContext
    {
        public J94013DbContext (DbContextOptions<J94013DbContext> options)
            : base(options)
        {
        }

        public DbSet<J94013.Models.Menu> Menu { get; set; }
        public DbSet<J94013.Models.CheckoutCustomers> CheckoutCustomers { get; set; }
        public DbSet<J94013.Models.Cart> Carts  { get; set; }
        public DbSet<J94013.Models.CartItem> CartItems { get; set; }
        public DbSet<J94013.Models.OrderHistory> OrderHistories { get; set; }
        public DbSet<J94013.Models.OrderItem> OrderItems { get; set; }

        [NotMapped]
        public DbSet<J94013.Models.CheckoutItem> CheckoutItems { get; set; }
        public DbSet<J94013.Models.Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<CartItem>().HasKey(t => new { t.MenuID, t.CartID });
            builder.Entity<OrderItem>().HasKey(t => new { t.MenuID, t.OrderNo });
            builder.Entity<Booking>().HasKey(t => new {t.BookingID});

        }
    }
}

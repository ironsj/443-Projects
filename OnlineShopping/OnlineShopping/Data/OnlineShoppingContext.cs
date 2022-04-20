#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineShopping.Models;

namespace OnlineShopping.Data
{
    public class OnlineShoppingContext : DbContext
    {
        public OnlineShoppingContext (DbContextOptions<OnlineShoppingContext> options)
            : base(options)
        {
        }

        public DbSet<OnlineShopping.Models.Shopper> Shoppers { get; set; }

        public DbSet<OnlineShopping.Models.Cart> Carts { get; set; }

        public DbSet<OnlineShopping.Models.Order> Orders { get; set; }

        public DbSet<OnlineShopping.Models.Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shopper>().ToTable("Shopper");
            modelBuilder.Entity<Cart>().ToTable("Cart");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Product>().ToTable("Product");
        }
    }
}

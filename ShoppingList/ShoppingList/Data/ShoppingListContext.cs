#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingList.Models;

namespace ShoppingList.Data
{
    public class ShoppingListContext : DbContext
    {
        public ShoppingListContext (DbContextOptions<ShoppingListContext> options)
            : base(options)
        {
        }

        public DbSet<ShoppingList.Models.Shopper> Shoppers { get; set; }

        public DbSet<ShoppingList.Models.List> Lists { get; set; }

        public DbSet<ShoppingList.Models.Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Shopper>().ToTable("Shopper");
            modelBuilder.Entity<List>().ToTable("List");
            modelBuilder.Entity<Item>().ToTable("Item");
        }
    }
}

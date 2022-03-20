using System.Linq;
using ShoppingList.Models;
using Microsoft.EntityFrameworkCore;

namespace ShoppingList.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ShoppingListContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ShoppingListContext>>()))
            {
                context.Database.EnsureCreated();


                if (context.Shoppers.Any())
                {
                    return;   // DB has been seeded
                }


                var shoppers = new Shopper[]
                {
                    new Shopper {
                        LastName = "WonderLand",
                        FirstName = "Alice",
                        Birthday = System.DateTime.Parse("2000-01-01"),
                        Email = "wonderlandalice@Example.com"
                    },
                    new Shopper {
                        LastName = "Thumb",
                        FirstName = "Thomas",
                        Birthday = System.DateTime.Parse("2000-01-01"),
                        Email = "thumbthomas@Example.com"
                    }
                };
                foreach (Shopper p in shoppers)
                {
                    context.Shoppers.Add(p);
                }
                context.SaveChanges();



                var lists = new List[]
                {
                    new List {
                        ShopperID = 1,
                        Name = "Candy",
                        TimeSlot = System.DateTime.Parse("2022-03-15"),
                        Subtotal = 2.00M,
                        Tax = 0M,
                        TotalCost= 2.00M
                    },
                   new List {
                        ShopperID = 1,
                        Name = "Fruit",
                        TimeSlot = System.DateTime.Parse("2022-03-16"),
                        Subtotal = 4.00M,
                        Tax = 0M,
                        TotalCost= 4.00M
                    },
                    new List {
                        ShopperID = 2,
                        Name = "Vegetables",
                        TimeSlot = System.DateTime.Parse("2022-03-17"),
                        Subtotal = 6.00M,
                        Tax = 0M,
                        TotalCost= 6.00M
                    },
                    new List {
                        ShopperID = 2,
                        Name = "Dry goods",
                        TimeSlot = System.DateTime.Parse("2022-03-18"),
                         Subtotal = 8.00M,
                        Tax = 0.48M,
                        TotalCost= 8.48M
                   }
                };
                foreach (List p in lists)
                {
                    context.Lists.Add(p);
                }
                context.SaveChanges();



 
                var items = new Item[]
                {
                    new Item {
                        ListID = 1,
                        ProductName = "Licorice",
                        UnitPrice = 2.00M,
                        Available = true,
                        Quantity = 1,
                        Cost = 2.00M,
                        TaxRate = 0,
                        Tax = 0M,
                        TotalCost = 2.00M
                    },
                    new Item {
                        ListID = 1,
                        ProductName = "M&Ms",
                        UnitPrice = 3.00M,
                        Available = false,
                        Quantity = 1,
                        Cost = 3.00M,
                        TaxRate = 0,
                        Tax = 0M,
                        TotalCost = 3.00M
                    },
                    new Item {
                        ListID = 2,
                        ProductName = "Apples",
                        UnitPrice = 2.00M,
                        Available = true,
                        Quantity = 2,
                        Cost = 4.00M,
                        TaxRate = 0,
                        Tax = 0M,
                        TotalCost = 4.00M
                    },
                    new Item {
                        ListID = 2,
                        ProductName = "Peaches",
                        UnitPrice = 3.00M,
                        Available = false,
                        Quantity = 2,
                        Cost = 6.00M,
                        TaxRate = 0,
                        Tax = 0M,
                        TotalCost = 6.00M
                    },
                    new Item {
                        ListID = 3,
                        ProductName = "Asparagus",
                        UnitPrice = 3.00M,
                        Available = true,
                        Quantity = 2,
                        Cost = 6.00M,
                        TaxRate = 0,
                        Tax = 0M,
                        TotalCost = 6.00M
                    },
                    new Item {
                        ListID = 3,
                        ProductName = "Brocolli",
                        UnitPrice = 3.00M,
                        Available = false,
                        Quantity = 3,
                        Cost = 9.00M,
                        TaxRate = 0,
                        Tax = 0M,
                        TotalCost = 9.00M
                    },
                    new Item {
                        ListID = 4,
                        ProductName = "Compass",
                        UnitPrice = 1.00M,
                        Available = true,
                        Quantity = 8,
                        Cost = 8.00M,
                        TaxRate = 0.06M,
                        Tax = 0.48M,
                        TotalCost = 8.48M
                    },
                    new Item {
                        ListID = 4,
                        ProductName = "Pencils",
                        UnitPrice = 1.00M,
                        Available = false,
                        Quantity = 2,
                        Cost = 2.00M,
                        TaxRate = 0.06M,
                        Tax = 0.12M,
                        TotalCost = 2.12M
                    },
                    new Item {
                        ListID = 4,
                        ProductName = "Straight edge",
                        UnitPrice = 1.00M,
                        Available = false,
                        Quantity = 3,
                        Cost = 3.00M,
                        TaxRate = 0.06M,
                        Tax = 0.18M,
                        TotalCost = 3.18M
                    }
                };
                foreach (Item p in items)
                {
                    context.Items.Add(p);
                }
                context.SaveChanges();

            }
        }
    }
}


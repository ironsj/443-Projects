using System.Linq;
using OnlineShopping.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineShopping.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new OnlineShoppingContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<OnlineShoppingContext>>()))
            {
                context.Database.EnsureCreated();

                // Look for any Products.
                if (context.Products.Any())
                {
                    return;   // DB has been seeded
                }

                var shoppers = new Shopper[]
                {
                     new Shopper {
                        LastName = "YourLastName",
                        FirstName = "YourFirstName",
                        Email = "youremailusername@example.com"
                     },
                   new Shopper {
                        LastName = "Churchill",
                        FirstName = "Winston",
                        Email = "churchillwinston@example.com"
                    },
                    new Shopper {
                        LastName = "Thatcher",
                        FirstName = "Margaret",
                        Email = "thatchermargaret@example.com"
                    },
                    new Shopper
                    {
                        LastName = "Irons",
                        FirstName = "Jake",
                        Email = "ironsj@mail.gvsu.edu"
                    }
                };
                foreach (Shopper s in shoppers)
                {
                    context.Shoppers.Add(s);
                }
                context.SaveChanges();


                var products = new Product[]
                {
                    new Product {
                        Name = "Apples",
                        Available = 10,
                        UnitPrice = 2.49M,
                        TaxRate = 0.0M,
                        DateStamp = System.DateTime.Now,
                        ShelfLife = 4
                    },
                    new Product {
                        Name = "Blueberries",
                        Available = 10,
                        UnitPrice = 1.99M,
                        TaxRate = 0.0M,
                        DateStamp = System.DateTime.Now,
                        ShelfLife = 3
                    },
                    new Product {
                        Name = "Cherries",
                        Available = 10,
                        UnitPrice = 4.99M,
                        TaxRate = 0.0M,
                        DateStamp = System.DateTime.Now,
                        ShelfLife = 3
                    },
                        new Product {
                        Name = "Rasberry Ice Tea",
                        Available = 10,
                        UnitPrice = 2.99M,
                        TaxRate = 0.0M,
                        DateStamp = System.DateTime.Now,
                        ShelfLife = 30
                    },
                    new Product {
                        Name = "Peaches",
                        Available = 10,
                        UnitPrice = 3.99M,
                        TaxRate = 0.0M,
                        DateStamp = System.DateTime.Now,
                        ShelfLife = 7
                    },
                    new Product {
                        Name = "Hammer",
                        Available = 10,
                        UnitPrice = 6.99M,
                        TaxRate = 0.10M
                    },
                    new Product {
                        Name = "Wrench",
                        Available = 10,
                        UnitPrice = 2.99M,
                        TaxRate = 0.07M
                    },
                    new Product {
                        Name = "Tape measure",
                        Available = 10,
                        UnitPrice = 2.99M,
                        TaxRate = 0.08M
                    },
                    new Product {
                        Name = "Masking tape",
                        Available = 10,
                        UnitPrice = 1.99M,
                        TaxRate = 0.06M
                    }
                };

                foreach (Product p in products)
                {
                    context.Products.Add(p);
                }
                context.SaveChanges();
            }
        }
    }
}

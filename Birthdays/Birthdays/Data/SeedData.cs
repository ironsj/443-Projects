using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Birthdays.Models;

namespace Birthdays.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BirthdaysContext(
                serviceProvider.GetRequiredService<DbContextOptions<BirthdaysContext>>()))
            {
                // Look for any people in the database
                if (context.People.Any())
                {
                    return;   // DB has been seeded
                }

                context.People.AddRange(
                    new Person
                    {
                        Name = "George Washington",
                        Birthday = DateTime.Parse("1732-1-25")
                    },
                    new Person
                    {
                        Name = "Abraham Lincoln",
                        Birthday = DateTime.Parse("1809-2-12")
                    },
                    new Person
                    {
                        Name = "Martin Luther King Jr.",
                        Birthday = DateTime.Parse("1929-1-15")
                    }
                );
                context.SaveChanges();
            }
        }
    }
}

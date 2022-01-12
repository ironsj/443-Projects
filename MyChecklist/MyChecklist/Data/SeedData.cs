

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using MyChecklist.Models;

namespace MyChecklist.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MyChecklistContext(
                serviceProvider.GetRequiredService<DbContextOptions<MyChecklistContext>>()))
            {
                // Look for any movies.
                if (context.Checks.Any())
                {
                    return;   // DB has been seeded
                }

                context.Checks.AddRange(
                    new Check
                    {
                        Unit = 0,
                        IsDone = false,
                        Topic = "Syllabus",
                        Task = "",
                        DueDate = DateTime.Parse("2022-1-10")
                    },
                    new Check
                    {
                        Unit = 1,
                        IsDone = false,
                        Topic = "MyChecklist",
                        Task = "Model, Scaffolding, Migration",
                        DueDate = DateTime.Parse("2022-1-10")
                    },
                    new Check
                    {
                        Unit = 1,
                        IsDone = false,
                        Topic = "MyChecklist",
                        Task = "Database Context & Controller",
                        DueDate = DateTime.Parse("2022-1-12")
                    },
                    new Check
                    {
                        Unit = 1,
                        IsDone = false,
                        Topic = "MyCheckList",
                        Task = "Seed database, actions & views",
                        DueDate = DateTime.Parse("2022-1-14")
                    },
                    new Check
                    {
                        Unit = 2,
                        IsDone = false,
                        Topic = "Movies",
                        Task = "Start",
                        DueDate = DateTime.Parse("2022-1-17")
                    },
                   new Check
                   {
                       Unit = 2,
                       IsDone = false,
                       Topic = "Movies",
                       Task = "Controller",
                       DueDate = DateTime.Parse("2022-1-17")
                   },
                   new Check
                   {
                       Unit = 2,
                       IsDone = false,
                       Topic = "Movies",
                       Task = "View & data dictionaries",
                       DueDate = DateTime.Parse("2022-1-17")
                   },
                   new Check
                   {
                       Unit = 2,
                       IsDone = false,
                       Topic = "Movies",
                       Task = "Model",
                       DueDate = DateTime.Parse("2022-1-17")
                   },
                   new Check
                   {
                       Unit = 2,
                       IsDone = false,
                       Topic = "Movies",
                       Task = "Database",
                       DueDate = DateTime.Parse("2022-1-24"),
                       Feature = "NuGet commands"
                   },
                   new Check
                   {
                       Unit = 2,
                       IsDone = false,
                       Topic = "Movies",
                       Task = "Actions & Views",
                       DueDate = DateTime.Parse("2022-1-24")
                   },
                   new Check
                   {
                       Unit = 2,
                       IsDone = false,
                       Topic = "Movies",
                       Task = "Add field - review",
                       DueDate = DateTime.Parse("2022-1-26")
                   },
                   new Check
                   {
                       Unit = 2,
                       IsDone = false,
                       Topic = "Movies",
                       Task = "Actions & Views",
                       DueDate = DateTime.Parse("2022-1-26")
                   },
                   new Check
                   {
                       Unit = 2,
                       IsDone = false,
                       Topic = "Movies",
                       Task = "Validation",
                       DueDate = DateTime.Parse("2022-1-28")
                   },
                   new Check
                   {
                       Unit = 2,
                       IsDone = false,
                       Topic = "Movies",
                       Task = "Details & Delete",
                       DueDate = DateTime.Parse("2022-1-28")
                   },
                   new Check
                   {
                       Unit = 3,
                       IsDone = false,
                       Topic = "University",
                       Task = "Scaffold Complex Data Model",
                       DueDate = DateTime.Parse("2022-1-31")
                   },
                   new Check
                   {
                       Unit = 3,
                       IsDone = false,
                       Topic = "University",
                       Task = "CRUD",
                       DueDate = DateTime.Parse("2022-02-02")
                   },
                   new Check
                   {
                       Unit = 3,
                       IsDone = false,
                       Topic = "University",
                       Task = "Sort & filter",
                       DueDate = DateTime.Parse("2022-02-04")
                   },
                   new Check
                   {
                       Unit = 3,
                       IsDone = false,
                       Topic = "University",
                       Task = "Review Migration",
                       DueDate = DateTime.Parse("2022-02-07")
                   },
                   new Check
                   {
                       Unit = 3,
                       IsDone = false,
                       Topic = "University",
                       Task = "Review Complex data model",
                       DueDate = DateTime.Parse("2022-02-09")
                   },
                   new Check
                   {
                       Unit = 3,
                       IsDone = false,
                       Topic = "University",
                       Task = "Read related data",
                       DueDate = DateTime.Parse("2022-02-14")
                   },
                   new Check
                   {
                       Unit = 3,
                       IsDone = false,
                       Topic = "University",
                       Task = "Update related data",
                       DueDate = DateTime.Parse("2022-02-16")
                   },
                   new Check
                   {
                       Unit = 3,
                       IsDone = false,
                       Topic = "University",
                       Task = "Concurency conflicts",
                       DueDate = DateTime.Parse("2022-02-18")
                   },
                   new Check
                   {
                       Unit = 3,
                       IsDone = false,
                       Topic = "University",
                       Task = "Inheritance",
                       DueDate = DateTime.Parse("2022-02-18")
                   },
                   new Check
                   {
                       Unit = 3,
                       IsDone = false,
                       Topic = "University",
                       Task = "Advanced topics",
                       DueDate = DateTime.Parse("2022-02-18")
                   },
                   new Check
                   {
                       Unit = 4,
                       IsDone = false,
                       Topic = "Banker",
                       Task = "Models & Controllers",
                       DueDate = DateTime.Parse("2022-02-21")
                   },
                   new Check
                   {
                       Unit = 4,
                       IsDone = false,
                       Topic = "Banker",
                       Task = "Actions & views",
                       DueDate = DateTime.Parse("2022-02-23")
                   },
                   new Check
                   {
                       Unit = 4,
                       IsDone = false,
                       Topic = "Banker",
                       Task = "Services w Dependency Injection",
                       DueDate = DateTime.Parse("2022-02-25")
                   },
                   new Check
                   {
                       Unit = 5,
                       IsDone = false,
                       Topic = "Sales Receipt",
                       Task = "Models & Controllers",
                       DueDate = DateTime.Parse("2022-02-28")
                   },
                   new Check
                   {
                       Unit = 5,
                       IsDone = false,
                       Topic = "Sales Receipt",
                       Task = "Details & Delete",
                       DueDate = DateTime.Parse("2022-03-02")
                   },
                   new Check
                   {
                       Unit = 5,
                       IsDone = false,
                       Topic = "Sales Receipt",
                       Task = "Actions & Route data",
                       DueDate = DateTime.Parse("2022-03-03")
                   },
                   new Check
                   {
                       Unit = 6,
                       IsDone = false,
                       Topic = "Checklist",
                       Task = "Authetication & navigation",
                       DueDate = DateTime.Parse("2022-03-14"),
                       Feature = "Navigation"
                   },
                   new Check
                   {
                       Unit = 6,
                       IsDone = false,
                       Topic = "Checklist",
                       Task = "Authetication",
                       DueDate = DateTime.Parse("2022-03-16"),
                       Feature = "Navigation"
                   },
                   new Check
                   {
                       Unit = 7,
                       IsDone = false,
                       Topic = "Online Shopping",
                       Task = "Model",
                       DueDate = DateTime.Parse("2022-03-14")
                   },
                   new Check
                   {
                       Unit = 7,
                       IsDone = false,
                       Topic = "Online shopping ",
                       Task = "Customers",
                       DueDate = DateTime.Parse("2022-03-16")
                   },
                  new Check
                  {
                      Unit = 7,
                      IsDone = false,
                      Topic = "Online Shopping",
                      Task = "Carts",
                      DueDate = DateTime.Parse("2022-03-18")
                  },
                   new Check
                   {
                       Unit = 7,
                       IsDone = false,
                       Topic = "Online shopping ",
                       Task = "Products",
                       DueDate = DateTime.Parse("2022-03-21")
                   },
                  new Check
                  {
                      Unit = 7,
                      IsDone = false,
                      Topic = "Online shopping ",
                      Task = "Orders",
                      DueDate = DateTime.Parse("2022-03-23")
                  },
                  new Check
                  {
                      Unit = 7,
                      IsDone = false,
                      Topic = "Online shopping ",
                      Task = "Customer actions & views",
                      DueDate = DateTime.Parse("2022-03-25")
                  },
                  new Check
                  {
                      Unit = 7,
                      IsDone = false,
                      Topic = "Online shopping ",
                      Task = "Cart actions & views",
                      DueDate = DateTime.Parse("2022-03-28")
                  },
                  new Check
                  {
                      Unit = 7,
                      IsDone = false,
                      Topic = "Online shopping ",
                      Task = "Product Actions & views",
                      DueDate = DateTime.Parse("2022-03-30")
                  },
                  new Check
                  {
                      Unit = 7,
                      IsDone = false,
                      Topic = "Online shopping ",
                      Task = "Order actions & views",
                      DueDate = DateTime.Parse("2022-04-01")
                  },
                  new Check
                  {
                      Unit = 8,
                      IsDone = false,
                      Topic = "Power BI",
                      Task = "",
                      DueDate = DateTime.Parse("2022-04-04")
                  },
                  new Check
                  {
                      Unit = 9,
                      IsDone = false,
                      Topic = "Appointments",
                      Task = "Customers",
                      DueDate = DateTime.Parse("2022-04-06")
                  },
                  new Check
                  {
                      Unit = 9,
                      IsDone = false,
                      Topic = "Appointments",
                      Task = "Parts",
                      DueDate = DateTime.Parse("2022-04-08")
                  },
                 new Check
                 {
                     Unit = 9,
                     IsDone = false,
                     Topic = "Appointments",
                     Task = "Labor",
                     DueDate = DateTime.Parse("2022-04-11")
                 },
                  new Check
                  {
                      Unit = 9,
                      IsDone = false,
                      Topic = "Appointments",
                      Task = "Parts actions & views",
                      DueDate = DateTime.Parse("2022-04-13")
                  },
                  new Check
                  {
                      Unit = 9,
                      IsDone = false,
                      Topic = "Appointments",
                      Task = "Labor actions & views",
                      DueDate = DateTime.Parse("2022-04-15")
                  });
                context.SaveChanges();
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Planner.Models;

namespace Planner.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new PlannerContext(
                serviceProvider.GetRequiredService<DbContextOptions<PlannerContext>>()))
            {
                // Look for any people in the database
                if (context.Appointments.Any())
                {
                    return;   // DB has been seeded
                }

                context.Appointments.AddRange(
                    new Appointment
                    {
                        Event = "443",
                        Day = DateTime.Parse("2022-1-10"),
                        TimeOfDay = DateTime.Parse("11:00 AM")
                    },
                    new Appointment
                    {
                        Event = "443",
                        Day = DateTime.Parse("2022-1-12"),
                        TimeOfDay = DateTime.Parse("11:00 AM")
                    },
                    new Appointment
                    {
                        Event = "443",
                        Day = DateTime.Parse("2022-1-14"),
                        TimeOfDay = DateTime.Parse("11:00 AM")
                    },
                    new Appointment
                    {
                        Event = "443",
                        Day = DateTime.Parse("2022-1-19"),
                        TimeOfDay = DateTime.Parse("11:00 AM")
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
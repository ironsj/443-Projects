#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Planner.Models;

namespace Planner.Data
{
    public class PlannerContext : DbContext
    {
        public PlannerContext (DbContextOptions<PlannerContext> options)
            : base(options)
        {
        }

        public DbSet<Planner.Models.Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>().ToTable("Appointment");
        }
    }
}

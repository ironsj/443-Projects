#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Birthdays.Models;

namespace Birthdays.Data
{
    public class BirthdaysContext : DbContext
    {
        public BirthdaysContext (DbContextOptions<BirthdaysContext> options)
            : base(options)
        {
        }

        public DbSet<Birthdays.Models.Person> People { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("Person");
        }
    }
}

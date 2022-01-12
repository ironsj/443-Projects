#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyChecklist.Models;

namespace MyChecklist.Data
{
    public class MyChecklistContext : DbContext
    {
        public MyChecklistContext (DbContextOptions<MyChecklistContext> options)
            : base(options)
        {
        }

        public DbSet<MyChecklist.Models.Check> Checks { get; set; }
    }
}

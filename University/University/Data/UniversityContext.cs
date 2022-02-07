#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using University.Models;

namespace University.Data
{
    public class UniversityContext : DbContext
    {
        public UniversityContext (DbContextOptions<UniversityContext> options)
            : base(options)
        {
        }

        public DbSet<University.Models.Course> Course { get; set; }

        public DbSet<University.Models.CourseAssignment> CourseAssignment { get; set; }

        public DbSet<University.Models.Department> Department { get; set; }

        public DbSet<University.Models.Enrollment> Enrollment { get; set; }

        public DbSet<University.Models.Instructor> Instructor { get; set; }

        public DbSet<University.Models.OfficeAssignment> OfficeAssignment { get; set; }

        public DbSet<University.Models.Student> Student { get; set; }
    }
}

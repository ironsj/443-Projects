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

        public DbSet<University.Models.Course> Courses { get; set; }

        public DbSet<University.Models.CourseAssignment> CourseAssignments { get; set; }

        public DbSet<University.Models.Department> Departments { get; set; }

        public DbSet<University.Models.Enrollment> Enrollments { get; set; }

        public DbSet<University.Models.Instructor> Instructors { get; set; }

        public DbSet<University.Models.OfficeAssignment> OfficeAssignments { get; set; }

        public DbSet<University.Models.Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Instructor>().ToTable("Instructor");
            modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment");
            modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment");
        }

    }
}

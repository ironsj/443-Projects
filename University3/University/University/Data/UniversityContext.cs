//#nullable disable
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using University.Models;

//namespace University.Data
//{
//    public class UniversityContext : DbContext
//    {
//        public UniversityContext (DbContextOptions<UniversityContext> options)
//            : base(options)
//        {
//        }

//        public DbSet<University.Models.Student> Student { get; set; }

//        public DbSet<University.Models.Course> Course { get; set; }

//        public DbSet<University.Models.CourseAssignment> CourseAssignment { get; set; }

//        public DbSet<University.Models.Department> Department { get; set; }

//        public DbSet<University.Models.Enrollment> Enrollment { get; set; }

//        public DbSet<University.Models.Instructor> Instructor { get; set; }

//        public DbSet<University.Models.OfficeAssignment> OfficeAssignment { get; set; }

//        public DbSet<University.Models.Person> Person { get; set; }
//    }
//}



using University.Models;
using Microsoft.EntityFrameworkCore;

namespace University.Data
{
    public class UniversityContext : DbContext
    {
        public UniversityContext(DbContextOptions<UniversityContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<CourseAssignment> CourseAssignments { get; set; }
        public DbSet<University.Models.Person> People { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Instructor>().ToTable("Instructor");
            modelBuilder.Entity<OfficeAssignment>().ToTable("OfficeAssignment");
            modelBuilder.Entity<CourseAssignment>().ToTable("CourseAssignment");
            modelBuilder.Entity<Person>().ToTable("Person");
        }
    }
}

using University.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace University.Data
{
    public static class DbInitializer
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new UniversityContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<UniversityContext>>()))
            {

                // context.Database.EnsureCreated();

                // Look for any students.
                if (context.Students.Any())
                {
                    return;   // DB has been seeded
                }
                else
                {
                    var students = new Student[]
                    {
                        new Student{FirstMidName="Jake",LastName="Irons",EnrollmentDate=DateTime.Parse("2017-08-27")},
                        new Student{FirstMidName="John",LastName="Adams",EnrollmentDate=DateTime.Parse("2005-09-01")},
                        new Student{FirstMidName="John",LastName="Hancock",EnrollmentDate=DateTime.Parse("2002-09-01")},
                        new Student{FirstMidName="Betsy",LastName="Ross",EnrollmentDate=DateTime.Parse("2003-09-01")},
                        new Student{FirstMidName="Martha",LastName="Washington",EnrollmentDate=DateTime.Parse("2002-09-01")},
                        new Student{FirstMidName="John",LastName="Witherspoon",EnrollmentDate=DateTime.Parse("2002-09-01")},
                        new Student{FirstMidName="Benjamin",LastName="Franklin",EnrollmentDate=DateTime.Parse("2001-09-01")},
                        new Student{FirstMidName="Samuel",LastName="Chase",EnrollmentDate=DateTime.Parse("2003-09-01")},
                        new Student{FirstMidName="Stephen",LastName="Hopkins",EnrollmentDate=DateTime.Parse("2005-09-01")}
                    };
                    foreach (Student s in students)
                    {
                        context.Students.Add(s);
                    }
                    context.SaveChanges();

                    var instructors = new Instructor[]
                     {
                         new Instructor { FirstMidName = "George",   LastName = "Washington",
                             HireDate = DateTime.Parse("1995-03-11") },
                         new Instructor { FirstMidName = "James",    LastName = "Garfield",
                             HireDate = DateTime.Parse("2002-07-06") },
                         new Instructor { FirstMidName = "Abraham",  LastName = "Lincoln",
                             HireDate = DateTime.Parse("1998-07-01") },
                         new Instructor { FirstMidName = "Winston",  LastName = "Churchill",
                             HireDate = DateTime.Parse("2001-01-15") },
                         new Instructor { FirstMidName = "Thomas",   LastName = "Jefferson",
                             HireDate = DateTime.Parse("2004-02-12") },
                         new Instructor { FirstMidName = "John",     LastName = "von Neumann",
                             HireDate = DateTime.Parse("1945-01-01") }
                     };

                    foreach (Instructor i in instructors)
                    {
                        context.Instructors.Add(i);
                    }
                    context.SaveChanges();

                    var departments = new Department[]
                    {
                        new Department { Name = "English",     Budget = 350000,
                            StartDate = DateTime.Parse("2007-09-01"),
                            InstructorID  = instructors.Single( i => i.LastName == "Washington").ID },
                        new Department { Name = "Mathematics", Budget = 100000,
                            StartDate = DateTime.Parse("2007-09-01"),
                            InstructorID  = instructors.Single( i => i.LastName == "Garfield").ID },
                        new Department { Name = "Engineering", Budget = 350000,
                            StartDate = DateTime.Parse("2007-09-01"),
                            InstructorID  = instructors.Single( i => i.LastName == "Lincoln").ID },
                        new Department { Name = "History",   Budget = 100000,
                            StartDate = DateTime.Parse("2007-09-01"),
                            InstructorID  = instructors.Single( i => i.LastName == "Churchill").ID },
                       new Department { Name = "Computing",   Budget = 1000000,
                            StartDate = DateTime.Parse("1991-11-01"),
                            InstructorID  = instructors.Single( i => i.LastName == "von Neumann").ID }
                    };

                    foreach (Department d in departments)
                    {
                        context.Departments.Add(d);
                    }
                    context.SaveChanges();


                    var courses = new Course[]
                    {
                        new Course{
                            CourseID=4050,
                            Title="Circuits",
                            DepartmentID=departments.Single( n => n.Name == "Engineering").DepartmentID,
                            Credits=3},
                        new Course{
                            CourseID=3022,
                            Title="Industrial Revolution",
                            DepartmentID=departments.Single( n => n.Name == "History").DepartmentID,
                            Credits=3},

                        new Course{
                            CourseID=3041,
                            Title="American Revolution",
                            DepartmentID=departments.Single( n => n.Name == "History").DepartmentID,
                            Credits=3},

                        new Course{
                            CourseID=4362,
                            Title="Calculus",
                            DepartmentID=departments.Single( n => n.Name == "Mathematics").DepartmentID,
                            Credits=4},
                        new Course{
                            CourseID=4141,
                            Title="Geometry",
                            DepartmentID=departments.Single( n => n.Name == "Mathematics").DepartmentID,
                            Credits=4},
                        new Course{
                            CourseID=1021,
                            Title="Composition",
                            DepartmentID=departments.Single( n => n.Name == "English").DepartmentID,
                            Credits=3},
                        new Course{
                            CourseID=1042,
                            Title="Literature",
                            DepartmentID=departments.Single( n => n.Name == "English").DepartmentID,
                            Credits=4},
                       new Course{
                            CourseID=4430,
                            Title="Web Apps",
                            DepartmentID=departments.Single( n => n.Name == "Computing").DepartmentID,
                            Credits=4}

                    };
                    foreach (Course c in courses)
                    {
                        context.Courses.Add(c);
                    }
                    context.SaveChanges();

                    var instructors2 = from s in context.Instructors
                                       select s;
                    var students2 = from s in context.Students
                                    select s;
                    var courses2 = from c in context.Courses
                                   select c;


                    var officeAssignments = new OfficeAssignment[]
                    {
                            new OfficeAssignment {
                                InstructorID = instructors2.Single( i => i.LastName == "Washington").ID,
                                Location = "Trinton 17" },
                            new OfficeAssignment {
                                InstructorID = instructors2.Single( i => i.LastName == "Garfield").ID,
                                Location = "White House 27" },
                            new OfficeAssignment {
                                InstructorID = instructors2.Single( i => i.LastName == "lincoln").ID,
                                Location = "Gettysburg 304" },
                    };

                    foreach (OfficeAssignment o in officeAssignments)
                    {
                        context.OfficeAssignments.Add(o);
                    }
                    context.SaveChanges();



                    var enrollments = new Enrollment[]
                    {
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Irons").ID,
                                CourseID = courses2.Single(c => c.Title == "Web Apps" ).CourseID,
                                Grade = Grade.A
                            },

                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Adams").ID,
                                CourseID = courses2.Single(c => c.Title == "Circuits" ).CourseID,
                                Grade = Grade.A
                            },

                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Adams").ID,
                                CourseID = courses2.Single(c => c.Title == "Calculus" ).CourseID,
                                Grade = Grade.A
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Adams").ID,
                                CourseID = courses2.Single(c => c.Title == "American Revolution" ).CourseID,
                                Grade = Grade.C
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Hancock").ID,
                                CourseID = courses2.Single(c => c.Title == "Industrial Revolution" ).CourseID,
                                Grade = Grade.B
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Hancock").ID,
                                CourseID = courses2.Single(c => c.Title == "Calculus" ).CourseID,
                                Grade = Grade.B
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Ross").ID,
                                CourseID = courses2.Single(c => c.Title == "Geometry" ).CourseID,
                                Grade = Grade.B
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Washington").ID,
                                CourseID = courses2.Single(c => c.Title == "Composition" ).CourseID,
                                Grade = Grade.B
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Witherspoon").ID,
                                CourseID = courses2.Single(c => c.Title == "Geometry" ).CourseID
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Franklin").ID,
                                CourseID = courses2.Single(c => c.Title == "American Revolution").CourseID,
                                Grade = Grade.B
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Chase").ID,
                                CourseID = courses2.Single(c => c.Title == "Geometry").CourseID,
                                Grade = Grade.B
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Hopkins").ID,
                                CourseID = courses2.Single(c => c.Title == "Composition").CourseID,
                                Grade = Grade.B
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Hopkins").ID,
                                CourseID = courses2.Single(c => c.Title == "Literature").CourseID,
                                Grade = Grade.B
                            }
                        };

                    foreach (Enrollment e in enrollments)
                    {
                        var enrollmentInDataBase = context.Enrollments.Where(
                            s =>
                                    s.Student.ID == e.StudentID &&
                                    s.Course.CourseID == e.CourseID).SingleOrDefault();
                        if (enrollmentInDataBase == null)
                        {
                            context.Enrollments.Add(e);
                        }
                    }
                    context.SaveChanges();


                    var instructors3 = from s in context.Instructors
                                       select s;
                    var students3 = from s in context.Students
                                    select s;
                    var courses3 = from c in context.Courses
                                   select c;
                    var courseInstructors = new CourseAssignment[]
                    {
                            new CourseAssignment {

                                CourseID = courses3.Single(c => c.Title == "Circuits" ).CourseID,
                                InstructorID = instructors3.Single(i => i.LastName == "von Neumann").ID
                            },
                            new CourseAssignment {

                                CourseID = courses3.Single(c => c.Title == "American Revolution" ).CourseID,
                                InstructorID = instructors3.Single(i => i.LastName == "Churchill").ID
                            },
                            new CourseAssignment {

                                CourseID = courses3.Single(c => c.Title == "American Revolution" ).CourseID,
                                InstructorID = instructors3.Single(i => i.LastName == "Washington").ID
                            },
                            new CourseAssignment {

                                CourseID = courses3.Single(c => c.Title == "Industrial Revolution" ).CourseID,
                                InstructorID = instructors3.Single(i => i.LastName == "Lincoln").ID
                            },
                            new CourseAssignment {

                                CourseID = courses3.Single(c => c.Title == "Calculus" ).CourseID,
                                InstructorID = instructors3.Single(i => i.LastName == "Garfield").ID
                            },
                            new CourseAssignment {

                                CourseID = courses3.Single(c => c.Title == "Geometry" ).CourseID,
                                InstructorID = instructors3.Single(i => i.LastName == "Garfield").ID
                            },
                            new CourseAssignment {

                                CourseID = courses2.Single(c => c.Title == "Composition" ).CourseID,
                                InstructorID = instructors2.Single(i => i.LastName == "Jefferson").ID
                             },
                            new CourseAssignment {

                                CourseID = courses3.Single(c => c.Title == "Literature" ).CourseID,
                                InstructorID = instructors3.Single(i => i.LastName == "Washington").ID
                            },
                            new CourseAssignment {

                                CourseID = courses3.Single(c => c.Title == "Web Apps" ).CourseID,
                                InstructorID = instructors3.Single(i => i.LastName == "von Neumann").ID
                            }
                    };

                    foreach (CourseAssignment ci in courseInstructors)
                    {

                        var courseAssignmentsInDataBase = context.CourseAssignments.Where(
                           s =>
                                   s.Instructor.ID == ci.InstructorID &&
                                   s.Course.CourseID == ci.CourseID).SingleOrDefault();
                        if (courseAssignmentsInDataBase == null)
                        {
                            context.CourseAssignments.Add(ci);
                        }
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
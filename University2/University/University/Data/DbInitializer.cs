
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
                        new Student{FirstMidName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2005-09-01")},
                        new Student{FirstMidName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01")},
                        new Student{FirstMidName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2003-09-01")},
                        new Student{FirstMidName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2002-09-01")},
                        new Student{FirstMidName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2002-09-01")},
                        new Student{FirstMidName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2001-09-01")},
                        new Student{FirstMidName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2003-09-01")},
                        new Student{FirstMidName="Nino",LastName="Olivetto",EnrollmentDate=DateTime.Parse("2005-09-01")},
                        new Student{FirstMidName="Jake",LastName="Irons",EnrollmentDate=DateTime.Parse("2017-08-27")}
                    };
                    foreach (Student s in students)
                    {
                        context.Students.Add(s);
                    }
                    context.SaveChanges();

                    var instructors = new Instructor[]
                     {
                         new Instructor { FirstMidName = "Kim",
                             LastName = "Abercrombie", HireDate = DateTime.Parse("1995-03-11") },
                         new Instructor { FirstMidName = "Fadi",    LastName = "Fakhouri",
                             HireDate = DateTime.Parse("2002-07-06") },
                         new Instructor { FirstMidName = "Roger",   LastName = "Harui",
                             HireDate = DateTime.Parse("1998-07-01") },
                         new Instructor { FirstMidName = "Candace", LastName = "Kapoor",
                             HireDate = DateTime.Parse("2001-01-15") },
                         new Instructor { FirstMidName = "Roger",   LastName = "Zheng",
                             HireDate = DateTime.Parse("2004-02-12") }
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
                            InstructorID  = instructors.Single( i => i.LastName == "Abercrombie").InstructorID },
                        new Department { Name = "Mathematics", Budget = 100000,
                            StartDate = DateTime.Parse("2007-09-01"),
                            InstructorID  = instructors.Single( i => i.LastName == "Fakhouri").InstructorID },
                        new Department { Name = "Engineering", Budget = 350000,
                            StartDate = DateTime.Parse("2007-09-01"),
                            InstructorID  = instructors.Single( i => i.LastName == "Harui").InstructorID },
                        new Department { Name = "Economics",   Budget = 100000,
                            StartDate = DateTime.Parse("2007-09-01"),
                            InstructorID  = instructors.Single( i => i.LastName == "Kapoor").InstructorID }
                    };

                    foreach (Department d in departments)
                    {
                        context.Departments.Add(d);
                    }
                    context.SaveChanges();


                    var courses = new Course[]
                    {
                        new Course{
                            CourseID=1050,
                            Title="Chemistry",
                            DepartmentID=departments.Single( n => n.Name == "Engineering").DepartmentID,
                            Credits=3},
                        new Course{
                            CourseID=4022,
                            Title="Microeconomics",
                            DepartmentID=departments.Single( n => n.Name == "Economics").DepartmentID,
                            Credits=3},

                        new Course{
                            CourseID=4041,
                            Title="Macroeconomics",
                            DepartmentID=departments.Single( n => n.Name == "Economics").DepartmentID,
                            Credits=3},

                        new Course{
                            CourseID=1045,
                            Title="Calculus",
                            DepartmentID=departments.Single( n => n.Name == "Mathematics").DepartmentID,
                            Credits=4},
                        new Course{
                            CourseID=3141,
                            Title="Trigonometry",
                            DepartmentID=departments.Single( n => n.Name == "Mathematics").DepartmentID,
                            Credits=4},
                        new Course{
                            CourseID=2021,
                            Title="Composition",
                            DepartmentID=departments.Single( n => n.Name == "English").DepartmentID,
                            Credits=3},
                        new Course{
                            CourseID=2042,
                            Title="Literature",
                            DepartmentID=departments.Single( n => n.Name == "English").DepartmentID,
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
                                InstructorID = instructors2.Single( i => i.LastName == "Fakhouri").InstructorID,
                                Location = "Smith 17" },
                            new OfficeAssignment {
                                InstructorID = instructors2.Single( i => i.LastName == "Harui").InstructorID,
                                Location = "Gowan 27" },
                            new OfficeAssignment {
                                InstructorID = instructors2.Single( i => i.LastName == "Kapoor").InstructorID,
                                Location = "Thompson 304" },
                    };

                    foreach (OfficeAssignment o in officeAssignments)
                    {
                        context.OfficeAssignments.Add(o);
                    }
                    context.SaveChanges();



                    var enrollments = new Enrollment[]
                    {
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Alexander").StudentID,
                                CourseID = courses2.Single(c => c.Title == "Chemistry" ).CourseID,
                                Grade = Grade.A
                            },

                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Alexander").StudentID,
                                CourseID = courses2.Single(c => c.Title == "Chemistry" ).CourseID,
                                Grade = Grade.A
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Alexander").StudentID,
                                CourseID = courses2.Single(c => c.Title == "Microeconomics" ).CourseID,
                                Grade = Grade.C
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Alexander").StudentID,
                                CourseID = courses2.Single(c => c.Title == "Macroeconomics" ).CourseID,
                                Grade = Grade.B
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Alonso").StudentID,
                                CourseID = courses2.Single(c => c.Title == "Calculus" ).CourseID,
                                Grade = Grade.B
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Alonso").StudentID,
                                CourseID = courses2.Single(c => c.Title == "Trigonometry" ).CourseID,
                                Grade = Grade.B
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Alonso").StudentID,
                                CourseID = courses2.Single(c => c.Title == "Composition" ).CourseID,
                                Grade = Grade.B
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Anand").StudentID,
                                CourseID = courses2.Single(c => c.Title == "Chemistry" ).CourseID
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Anand").StudentID,
                                CourseID = courses2.Single(c => c.Title == "Microeconomics").CourseID,
                                Grade = Grade.B
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Barzdukas").StudentID,
                                CourseID = courses2.Single(c => c.Title == "Chemistry").CourseID,
                                Grade = Grade.B
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Li").StudentID,
                                CourseID = courses2.Single(c => c.Title == "Composition").CourseID,
                                Grade = Grade.B
                            },
                            new Enrollment {
                                StudentID = students2.Single(s => s.LastName == "Justice").StudentID,
                                CourseID = courses2.Single(c => c.Title == "Literature").CourseID,
                                Grade = Grade.B
                            }
                        };

                    foreach (Enrollment e in enrollments)
                    {
                        var enrollmentInDataBase = context.Enrollments.Where(
                            s =>
                                    s.Student.StudentID == e.StudentID &&
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
                                
                                CourseID = courses3.Single(c => c.Title == "Chemistry" ).CourseID,
                                InstructorID = instructors3.Single(i => i.LastName == "Kapoor").InstructorID
                            },
                            new CourseAssignment {

                                CourseID = courses3.Single(c => c.Title == "Chemistry" ).CourseID,
                                InstructorID = instructors3.Single(i => i.LastName == "Harui").InstructorID
                            },
                            new CourseAssignment {

                                CourseID = courses3.Single(c => c.Title == "Microeconomics" ).CourseID,
                                InstructorID = instructors3.Single(i => i.LastName == "Zheng").InstructorID
                            },
                            new CourseAssignment {

                                CourseID = courses3.Single(c => c.Title == "Macroeconomics" ).CourseID,
                                InstructorID = instructors3.Single(i => i.LastName == "Zheng").InstructorID
                            },
                            new CourseAssignment {

                                CourseID = courses3.Single(c => c.Title == "Calculus" ).CourseID,
                                InstructorID = instructors3.Single(i => i.LastName == "Fakhouri").InstructorID
                            },
                            new CourseAssignment {

                                CourseID = courses3.Single(c => c.Title == "Trigonometry" ).CourseID,
                                InstructorID = instructors3.Single(i => i.LastName == "Harui").InstructorID
                            },
                            new CourseAssignment {
                                                                
                                CourseID = courses2.Single(c => c.Title == "Composition" ).CourseID,
                                InstructorID = instructors2.Single(i => i.LastName == "Abercrombie").InstructorID
                             },
                            new CourseAssignment {

                                CourseID = courses3.Single(c => c.Title == "Literature" ).CourseID,
                                InstructorID = instructors3.Single(i => i.LastName == "Abercrombie").InstructorID
                            }
                    };

                    foreach (CourseAssignment ci in courseInstructors)
                    {

                        var courseAssignmentsInDataBase = context.CourseAssignments.Where(
                           s =>
                                   s.Instructor.InstructorID == ci.InstructorID &&
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
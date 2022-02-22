
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Models
{
    public class Course
    {
        #region
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Number")]
        #endregion
        public int CourseID { get; set; }
        #region
        [StringLength(50, MinimumLength = 3)]
        #endregion
        public string? Title { get; set; }
        #region
        [Range(0, 5)]
        #endregion
        public int Credits { get; set; }
        public int? DepartmentID { get; set; }


        public Department? Department { get; set; }
        public ICollection<Enrollment>? Enrollments { get; set; }
        public ICollection<CourseAssignment>? CourseAssignments { get; set; }
    }
}
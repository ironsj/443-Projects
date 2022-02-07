using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }
        #region
        [StringLength(50, MinimumLength = 3)]
        #endregion
        public string Name { get; set; }
        #region
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        #endregion
        public decimal Budget { get; set; }
        #region
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        #endregion
        public DateTime StartDate { get; set; }

        public int? InstructorID { get; set; }

        public Instructor Administrator { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
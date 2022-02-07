using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Models
{
    public class Instructor
    {
        public int ID { get; set; }
        #region
        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50)]
        #endregion
        public string LastName { get; set; }
        #region
        [Required]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        [StringLength(50)]
        #endregion
        public string FirstMidName { get; set; }
        #region
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        #endregion
        public DateTime HireDate { get; set; }
        #region
        [Display(Name = "Full Name")]
        #endregion
        public string FullName
        {
            get { return LastName + ", " + FirstMidName; }
        }

        public ICollection<CourseAssignment> CourseAssignments { get; set; }
        public OfficeAssignment OfficeAssignment { get; set; }
    }
}
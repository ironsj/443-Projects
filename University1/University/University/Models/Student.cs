using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Models
{
    public class Student
    {
        public int ID { get; set; }
        #region
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        #endregion
        public string LastName { get; set; }
        #region
        [Required]
        [StringLength(50)]
        [Column("FirstName")]
        [Display(Name = "First Name")]
        #endregion
        public string FirstMidName { get; set; }
        #region
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        #endregion
        public DateTime EnrollmentDate { get; set; }
        #region
        [Display(Name = "Full Name")]
        #endregion
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstMidName;
            }
        }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
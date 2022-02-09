using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace University.Models
{
    public class OfficeAssignment
    {
        [Key]
        public int InstructorID { get; set; }
        #region
        [StringLength(50)]
        [Display(Name = "Office Location")]
        #endregion
        public string Location { get; set; }

        public Instructor Instructor { get; set; }
    }
}

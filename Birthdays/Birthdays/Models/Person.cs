using System.ComponentModel.DataAnnotations;

namespace Birthdays.Models
{
    public class Person
    {
        public int PersonID { get; set; }
        public string Name { get; set; }
        #region
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}",
            ApplyFormatInEditMode = true)]
        #endregion
        public DateTime Birthday { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Birthdays.Models
{
    public class Person
    {

        public int PersonID { get; set; }

        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]

        public DateTime DateTime { get; set; }
    }
}

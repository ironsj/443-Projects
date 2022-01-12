
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyChecklist.Models
{
    public class Check
    {
        public int CheckID { get; set; }
        public bool IsDone { get; set; }
        public int Unit { get; set; }
        [Required]
        public string Topic { get; set; }
        public string? Task { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}",
            ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }
        public double? HoursInvested { get; set; }
        public string? Feature { get; set; }
    }
}
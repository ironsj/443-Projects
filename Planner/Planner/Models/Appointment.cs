using System.ComponentModel.DataAnnotations;

namespace Planner.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public string? Event { get; set; }
        #region
        [DataType(DataType.Date)]
        #endregion
        public DateTime? Day { get; set; }
        #region
        [DataType(DataType.Time)]
        #endregion
        public DateTime? TimeOfDay { get; set; }
    }
}

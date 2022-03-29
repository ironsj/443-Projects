using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.Models
{
    public class Transaction
    {
        public enum Actions
        {
            deposit, withdraw, interest
        }

        public int TransactionID { get; set; }
        public int AccountID { get; set; }
        #region
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "Transaction time")]
        #endregion
        public DateTime? TimeSlot { get; set; }
        public Actions? Action{ get; set; }
        #region
        //  [Required]
        [Range(0.00, double.MaxValue,
             ErrorMessage = "Please enter a positive deposit")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        #endregion
        public decimal? Amount { get; set; }
        #region
        //[Required]
        [Range(0.00, double.MaxValue,
             ErrorMessage = "Please enter a positive deposit")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        #endregion
        public decimal? NewBalance { get; set; }


        public Account? Account { get; set; }
    }
}

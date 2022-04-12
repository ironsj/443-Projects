using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.Models
{
    public class Bill
    {
        public int BillID { get; set; }
        public int CustomerID { get; set; }
        public int? AccountID { get; set; }
        public string? Creditor { get; set; }
        public string? Contact { get; set; }
        public string? Detail { get; set; }
        #region
        [Required]
        [Range(0.00, double.MaxValue,
             ErrorMessage = "Please enter a positive amount due")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        #endregion
        public decimal AmountDue { get; set; }
        #region
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Due date")]
        #endregion
        public DateTime DueDate { get; set; }
        public int? PayerID { get; set; }

        #region
        [Required]
        [Range(0.00, double.MaxValue,
             ErrorMessage = "Please enter a positive amount paid")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        #endregion
        public decimal AmountPaid { get; set; }

        #region
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date paid")]
        #endregion
        public DateTime? DatePaid { get; set; }
        public bool Paid { get; set; }
        public string? ConfirmationNumber { get; set; }

        public Account? Account { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<Transaction>? Transactions { get; }
        public ICollection<Transfer>? Transfers { get; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.Models
{
    public class Account
    {
        public enum Kinds
        {
            checking, savings, credit, debit, bill, other
        }

        public int AccountID { get; set; }
        public int CustomerID { get; set; }
        #region
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Transaction time")]
        #endregion
        public DateTime AccountDate { get; set; }
        public string? Name { get; set; }
        public Kinds? Kind { get; set; }
        #region
        [Required]
        [Range(double.MinValue, double.MaxValue,
             ErrorMessage = "Please enter a positive price")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        #endregion
        public decimal Balance { get; set; }
        #region
        [Required]
        [Range(0.00, double.MaxValue,
             ErrorMessage = "Please enter an interest rate")]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        #endregion
        public decimal InterestRate { get; set; }


        public Customer? Customer { get; set; }

        public ICollection<Transaction>? Transactions { get; }
        public ICollection<Transfer>? Transfers { get; }
    }
}

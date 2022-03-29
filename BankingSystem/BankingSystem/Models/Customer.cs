using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        #region
        [Display(Name = "Full Name")]
        #endregion
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }
        #region
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        #endregion
        public DateTime Birthday { get; set; }
        #region
        [Display(Name = "Email address")]
        //[Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        //[KeyAttribute(ErrorMessage = "An account with this email address already exists")]
        #endregion
        public string? Email { get; set; }
        //public EmailAddressAttribute? EmailAddress { get; set; }

        public ICollection<Account>? Accounts { get; }
    }
}
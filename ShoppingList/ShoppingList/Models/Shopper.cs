using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Email regular expressions
/// https://www.formget.com/regular-expression-for-email/
/// 
/// </summary>
/// 
namespace ShoppingList.Models
{
    public class Shopper
    {
        public int ShopperID { get; set; }
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

        public ICollection<List>? Lists { get; }
    }
}

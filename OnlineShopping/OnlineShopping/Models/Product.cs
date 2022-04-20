using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShopping.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        #region
        [StringLength(50, MinimumLength = 3)]
        #endregion
        public string? Name { get; set; }
        #region
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be non-negative")]
        #endregion
        public int Available { get; set; }
        #region
        [Range(0.00, double.MaxValue, ErrorMessage = "UnitPrice must be non-negative")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        #endregion
        public decimal UnitPrice { get; set; }
        #region
        [Range(0.00, double.MaxValue, ErrorMessage = "TaxRate must be non-negative")]
        [Column(TypeName = "decimal(18, 2)")]
        #endregion
        public decimal TaxRate { get; set; }
        #region
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd MM yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "DateStamp Date")]
        #endregion
        public DateTime? DateStamp { get; set; }
        public int? ShelfLife { get; set; }

        public ICollection<Order>? Orders { get; set; }
    }
}
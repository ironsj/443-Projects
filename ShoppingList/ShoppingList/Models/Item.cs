
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingList.Models
{
    public class Item
    {
        public int ItemID { get; set; }
        public int ListID { get; set; }

        public string? ProductName { get; set; }
        #region
        [Required]
        [Range(0.00, (double)decimal.MaxValue)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayName("Price ($)")]
        #endregion
        public decimal UnitPrice { get; set; }
        public bool Available { get; set; }
        public int Quantity { get; set; }
        #region
        [Range(0.00, (double)decimal.MaxValue)]
        [Column(TypeName = "decimal(18, 2)")]
        [DataType(DataType.Currency)]
        #endregion
        public decimal? Cost { get; set; }
        #region
        [Range(0.00, (double)decimal.MaxValue)]
        [Column(TypeName = "decimal(18, 2)")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:P0}")]
        #endregion
        public decimal TaxRate { get; set; }
        #region
        [Range(0.00, (double)decimal.MaxValue)]
        [Column(TypeName = "decimal(18, 2)")]
        [DataType(DataType.Currency)]
        #endregion
        public decimal? Tax { get; set; }
        #region
        [Range(0.00, (double)decimal.MaxValue)]
        [Column(TypeName = "decimal(18, 2)")]
        [DataType(DataType.Currency)]
        #endregion
        public decimal? TotalCost { get; set; }

        public List? List { get; set; }
    }
}

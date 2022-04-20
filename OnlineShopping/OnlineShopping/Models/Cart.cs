using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace OnlineShopping.Models
{
    public class Cart
    {
        public int CartID { get; set; }
        public int ShopperID { get; set; }
        #region
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
        [Display(Name = "Transaction time")]
        #endregion
        public DateTime? TimeSlot { get; set; }
        #region
        [Range(0.00, double.MaxValue, ErrorMessage = "Subtotal must be non-negative.")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        #endregion
        public decimal? Subtotal { get; set; }
        #region
        [Range(0.00, double.MaxValue, ErrorMessage = "Tax must be non-negative.")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        #endregion
        public decimal? Tax { get; set; }
        #region
        [Range(0.00, double.MaxValue, ErrorMessage = "TotalCost must be non-negative")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        #endregion
        public decimal? TotalCost { get; set; }
        public bool CheckedOut { get; set; }


        public Shopper? Shopper { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models
{

    public class Movie
    {
        public int Id { get; set; }

        #region
        [StringLength(60, MinimumLength = 3)]
        [Required]
        #endregion
        public string Title { get; set; }
        #region
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        #endregion
        public DateTime ReleaseDate { get; set; }
        #region
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        #endregion
        public decimal Price { get; set; }
        #region
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Required]
        [StringLength(30)]
        #endregion
        public string Genre { get; set; }
        #region
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [StringLength(5)]
        [Required]
        #endregion
        public string Rating { get; set; }
    }
}

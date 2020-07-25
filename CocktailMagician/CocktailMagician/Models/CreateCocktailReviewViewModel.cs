using System;
using System.ComponentModel.DataAnnotations;

namespace CocktailMagician.Web.Models
{
    public class CreateCocktailReviewViewModel
    {
        public CreateCocktailReviewViewModel()
        {

        }
        [Required]
        [Range(0, 5, ErrorMessage = "Raitig is between 0-5")]
        public double Rating { get; set; }

        [MaxLength(500, ErrorMessage = "Use maximum 500 chars for your comment")]
        public string Comment { get; set; }
        public int CocktailId { get; set; }
        public int AuthorId { get; set; }
    }
}

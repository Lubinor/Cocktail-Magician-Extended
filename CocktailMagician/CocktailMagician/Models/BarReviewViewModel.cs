using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CocktailMagician.Web.Models
{
    public class BarReviewViewModel
    {
        public BarReviewViewModel()
        {

        }
        [Required]
        //[Range(1, 5, ErrorMessage = "Ratig is between 1-5")]
        public double Rating { get; set; }
        //[MaxLength(500, ErrorMessage = "Use maximum 500 chars for your comment")]

        public string Comment { get; set; }
        public int BarId { get; set; }

        [DisplayName("Bar name")]
        public string BarName { get; set; }
        public int AuthorId { get; set; }
        public string Author { get; set; }
    }
}

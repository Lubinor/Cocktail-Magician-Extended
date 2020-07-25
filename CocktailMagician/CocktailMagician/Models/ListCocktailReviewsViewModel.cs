using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CocktailMagician.Web.Models
{
    public class ListCocktailReviewsViewModel
    {
        [DisplayName("All Reviews")]
        public ICollection<CocktailReviewViewModel> AllCocktailReviews { get; set; }
        [DisplayName("Comment")]
        public string ReviewComment { get; set; }
        [DisplayName("Author")]
        public string Author { get; set; }
        [DisplayName("Cocktail")]
        public string Cocktail { get; set; }
        [DisplayName("Rating")]
        public double Rating { get; set; }
        public int CocktailId { get; set; }

    }
}

using System.Collections.Generic;
using System.ComponentModel;

namespace CocktailMagician.Web.Models
{
    public class ListBarReviewsViewModel
    {
        [DisplayName("All Reviews")]
        public ICollection<BarReviewViewModel> AllBarReviews { get; set; }
        [DisplayName("Comment")]
        public string ReviewComment { get; set; }
        [DisplayName("Author")]
        public string Author { get; set; }
        [DisplayName("Bar")]
        public string Bar { get; set; }
        [DisplayName("Rating")]
        public double Rating { get; set; }
        public int BarId { get; set; }
    }
}

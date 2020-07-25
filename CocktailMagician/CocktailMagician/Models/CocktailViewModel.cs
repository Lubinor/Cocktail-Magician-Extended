using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CocktailMagician.Web.Models
{
    public class CocktailViewModel
    {
        public CocktailViewModel()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }

        [DisplayName("Average Rating")]
        [MaxLength(30)]
        public double AverageRating { get; set; }
        public ICollection<IngredientViewModel> Ingredients { get; set; } = new List<IngredientViewModel>();
        public ICollection<BarViewModel> Bars { get; set; } = new List<BarViewModel>();

        [DisplayName("Ingredients")]
        public string IngredientNames { get; set; }

        [DisplayName("Bars")]
        public string BarNames { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageSource { get; set; }
    }
}

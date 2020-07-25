using System;
using System.Collections.Generic;

namespace CocktailMagician.Models
{
    public class Cocktail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double AverageRating { get; set; }
        public ICollection<IngredientsCocktails> IngredientsCocktails { get; set; } = new HashSet<IngredientsCocktails>();
        public ICollection<BarsCocktails> CocktailBars { get; set; } = new HashSet<BarsCocktails>();
        public ICollection<CocktailsUsersReviews> Reviews { get; set; } = new HashSet<CocktailsUsersReviews>();
        public DateTime? CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageSource { get; set; }
    }
}

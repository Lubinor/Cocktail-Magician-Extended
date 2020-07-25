using System;
using System.Collections.Generic;

namespace CocktailMagician.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<IngredientsCocktails> IngredientsCocktails { get; set; } = new HashSet<IngredientsCocktails>();
        public DateTime? CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageSource { get; set; }
    }
}

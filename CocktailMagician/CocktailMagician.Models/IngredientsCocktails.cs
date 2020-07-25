namespace CocktailMagician.Models
{
    public class IngredientsCocktails
    {
        public int IngredientId { get; set; }
        public int CocktailId { get; set; }

        public Ingredient Ingredient { get; set; }
        public Cocktail Cocktail { get; set; }

        public bool IsDeleted { get; set; }
    }
}

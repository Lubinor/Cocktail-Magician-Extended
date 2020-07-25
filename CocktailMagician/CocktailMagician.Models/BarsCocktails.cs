namespace CocktailMagician.Models
{
    public class BarsCocktails
    {
        public int BarId { get; set; }
        public int CocktailId { get; set; }

        public Bar Bar { get; set; }
        public Cocktail Cocktail { get; set; }

        public bool IsDeleted { get; set; }
    }
}

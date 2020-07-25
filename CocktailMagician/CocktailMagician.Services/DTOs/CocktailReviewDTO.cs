namespace CocktailMagician.Services.DTOs
{
    public class CocktailReviewDTO
    {
        public CocktailReviewDTO()
        {

        }
        public double Rating { get; set; }
        public string Comment { get; set; }
        public int CocktailId { get; set; }
        public string CocktailName { get; set; }
        public int AuthorId { get; set; }
        public string Author { get; set; }
    }
}

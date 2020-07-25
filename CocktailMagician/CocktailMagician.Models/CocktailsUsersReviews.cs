using System;
using System.ComponentModel.DataAnnotations;

namespace CocktailMagician.Models
{
    public class CocktailsUsersReviews
    {
        public int CocktailId { get; set; }
        public int UserId { get; set; }

        public Cocktail Cocktail { get; set; }
        public User User { get; set; }

        [Range(0, 5)]
        public double Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
    }
}

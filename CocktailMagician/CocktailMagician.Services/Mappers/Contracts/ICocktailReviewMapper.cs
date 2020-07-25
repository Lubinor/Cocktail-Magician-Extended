using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;

namespace CocktailMagician.Services.Mappers.Contracts
{
    public interface ICocktailReviewMapper
    {
        public CocktailReviewDTO MapToCocktailReviewDTO(CocktailsUsersReviews review);
        public CocktailsUsersReviews MapToCocktailReview(CocktailReviewDTO reviewDTO);
    }
}

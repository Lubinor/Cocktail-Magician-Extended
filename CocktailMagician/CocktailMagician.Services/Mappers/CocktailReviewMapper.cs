using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;

namespace CocktailMagician.Services.Mappers
{
    public class CocktailReviewMapper : ICocktailReviewMapper
    {
        public CocktailReviewDTO MapToCocktailReviewDTO(CocktailsUsersReviews review)
        {
            CocktailReviewDTO reviewDTO = new CocktailReviewDTO
            {
                Rating = review.Rating,
                Comment = review.Comment,
                CocktailId = review.CocktailId,
                CocktailName = review.Cocktail.Name,
                AuthorId = review.UserId,
                Author = review.User.UserName
            };

            return reviewDTO;
        }

        public CocktailsUsersReviews MapToCocktailReview(CocktailReviewDTO reviewDTO)
        {
            CocktailsUsersReviews review = new CocktailsUsersReviews
            {
                Rating = reviewDTO.Rating,
                Comment = reviewDTO.Comment,
                CocktailId = reviewDTO.CocktailId,
                UserId = reviewDTO.AuthorId
            };

            return review;
        }

    }
}

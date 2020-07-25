using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;

namespace CocktailMagician.Services.Mappers
{
    public class BarReviewMapper : IBarReviewMapper
    {
        public BarReviewDTO MapToBarReviewDTO(BarsUsersReviews review)
        {
            BarReviewDTO reviewDTO = new BarReviewDTO
            {
                Rating = review.Rating,
                Comment = review.Comment,
                BarId = review.BarId,
                AuthorId = review.UserId,
                BarName = review.Bar.Name,
                Author = review.User.UserName
            };

            return reviewDTO;
        }
        public BarsUsersReviews MapToBarReview(BarReviewDTO reviewDTO)
        {
            BarsUsersReviews review = new BarsUsersReviews
            {
                Rating = reviewDTO.Rating,
                Comment = reviewDTO.Comment,
                BarId = reviewDTO.BarId,
                UserId = reviewDTO.AuthorId
            };

            return review;
        }
    }
}

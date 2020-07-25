using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;

namespace CocktailMagician.Services.Mappers.Contracts
{
    public interface IBarReviewMapper
    {
        public BarReviewDTO MapToBarReviewDTO(BarsUsersReviews barReview);
        public BarsUsersReviews MapToBarReview(BarReviewDTO barReviewDTO);
    }
}

using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.ValidationModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CocktailMagician.Services.Contracts
{
    public interface IBarReviewService
    {
        Task<ICollection<BarReviewDTO>> GetAllBarReviewsAsync(int barId);
        Task<ICollection<BarReviewDTO>> GetAllUserReviewsAsync(int userId);
        Task<BarReviewDTO> GetBarReviewAsync(int barId, int userId);
        Task<BarReviewDTO> CreateBarReviewAsync(BarReviewDTO barReviewDTO);
        Task<BarReviewDTO> UpdateBarReviewAsync(int barId, int userId, BarReviewDTO barReviewDTO);
        Task<bool> DeleteBarReviewAsync(int barId, int userId);
        double GetBarRating(int barId);
        ValidationModel ValidateBarReview(BarReviewDTO barReviewDTO);
        bool BarReviewIsUnique(BarReviewDTO barReviewDTO);
    }
}

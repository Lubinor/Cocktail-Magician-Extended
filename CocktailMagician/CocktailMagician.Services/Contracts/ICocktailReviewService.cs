using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.ValidationModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CocktailMagician.Services.Contracts
{
    public interface ICocktailReviewService
    {
        Task<ICollection<CocktailReviewDTO>> GetAllCocktailReviewsAsync(int cocktailId);
        Task<ICollection<CocktailReviewDTO>> GetAllUserReviewsAsync(int userId);
        Task<CocktailReviewDTO> GetCocktailReviewAsync(int cocktailId, int userId);
        Task<CocktailReviewDTO> CreateCocktailReviewAsync(CocktailReviewDTO cocktailReviewDTO);
        Task<CocktailReviewDTO> UpdateCocktailReviewAsync(int cocktailId, int userId, CocktailReviewDTO cocktailReviewDTO);
        Task<bool> DeleteCocktailReviewAsync(int cocktailId, int userId);
        double GetCocktailRating(int cocktailId);
        ValidationModel ValidateCocktailReview(CocktailReviewDTO cocktailReviewDTO);
        bool CocktailReviewIsUnique(CocktailReviewDTO cocktailReviewDTO);
    }
}

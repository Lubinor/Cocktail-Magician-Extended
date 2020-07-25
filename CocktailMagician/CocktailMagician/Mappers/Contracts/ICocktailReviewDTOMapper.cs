using CocktailMagician.Services.DTOs;
using CocktailMagician.Web.Models;

namespace CocktailMagician.Web.Mappers.Contracts
{
    public interface ICocktailReviewDTOMapper
    {
        public CocktailReviewViewModel MapToVMFromDTO(CocktailReviewDTO cocktailReviewDTO);
        public CocktailReviewDTO MapToDTOFromVM(CocktailReviewViewModel cocktailReviewVM);
        public CocktailReviewDTO MapToDTOFromVM(CreateCocktailReviewViewModel createCocktailReviewVM);
    }
}

using CocktailMagician.Services.DTOs;
using CocktailMagician.Web.Mappers.Contracts;
using CocktailMagician.Web.Models;

namespace CocktailMagician.Web.Mappers
{
    public class CocktailReviewDTOMapper : ICocktailReviewDTOMapper
    {
        public CocktailReviewDTOMapper()
        {

        }
        public CocktailReviewViewModel MapToVMFromDTO(CocktailReviewDTO cocktailReviewDTO)
        {
            var reviewVM = new CocktailReviewViewModel
            {
                Rating = cocktailReviewDTO.Rating,
                Comment = cocktailReviewDTO.Comment,
                CocktailId = cocktailReviewDTO.CocktailId,
                AuthorId = cocktailReviewDTO.AuthorId,
                CocktailName = cocktailReviewDTO.CocktailName,
                Author = cocktailReviewDTO.Author
            };

            return reviewVM;
        }
        public CocktailReviewDTO MapToDTOFromVM(CocktailReviewViewModel cocktailReviewVM)
        {
            var reviewDTO = new CocktailReviewDTO
            {
                Comment = cocktailReviewVM.Comment,
                CocktailId = cocktailReviewVM.CocktailId,
                AuthorId = cocktailReviewVM.AuthorId,
                CocktailName = cocktailReviewVM.CocktailName,
                Author = cocktailReviewVM.Author,
                Rating = cocktailReviewVM.Rating
            };

            return reviewDTO;
        }

        public CocktailReviewDTO MapToDTOFromVM(CreateCocktailReviewViewModel createCocktailReviewVM)
        {
            var reviewDTO = new CocktailReviewDTO
            {
                Comment = createCocktailReviewVM.Comment,
                CocktailId = createCocktailReviewVM.CocktailId,
                AuthorId = createCocktailReviewVM.AuthorId,
                Rating = createCocktailReviewVM.Rating
            };

            return reviewDTO;
        }
    }
}

using CocktailMagician.Services.DTOs;
using CocktailMagician.Web.Mappers.Contracts;
using CocktailMagician.Web.Models;

namespace CocktailMagician.Web.Mappers
{
    public class BarReviewDTOMapper : IBarReviewDTOMapper
    {
        public BarReviewDTOMapper()
        {

        }

        public BarReviewViewModel MapToVMFromDTO(BarReviewDTO barReviewDTO)
        {
            BarReviewViewModel reviewVM = new BarReviewViewModel
            {
                Rating = barReviewDTO.Rating,
                Comment = barReviewDTO.Comment,
                BarId = barReviewDTO.BarId,
                AuthorId = barReviewDTO.AuthorId,
                BarName = barReviewDTO.BarName,
                Author = barReviewDTO.Author
            };

            return reviewVM;
        }
 
        public BarReviewDTO MapToDTOFromVM(BarReviewViewModel barReviewVM)
        {
            BarReviewDTO reviewDTO = new BarReviewDTO
            {
                Comment = barReviewVM.Comment,
                BarId = barReviewVM.BarId,
                AuthorId = barReviewVM.AuthorId,
                BarName = barReviewVM.BarName,
                Author = barReviewVM.Author,
                Rating = barReviewVM.Rating
            };

            return reviewDTO;
        }
    }
}

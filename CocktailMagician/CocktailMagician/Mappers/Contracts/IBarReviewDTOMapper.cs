using CocktailMagician.Services.DTOs;
using CocktailMagician.Web.Models;

namespace CocktailMagician.Web.Mappers.Contracts
{
    public interface IBarReviewDTOMapper
    {
        public BarReviewViewModel MapToVMFromDTO(BarReviewDTO barReviewDTO);
        public BarReviewDTO MapToDTOFromVM(BarReviewViewModel barReviewVM);
    }
}

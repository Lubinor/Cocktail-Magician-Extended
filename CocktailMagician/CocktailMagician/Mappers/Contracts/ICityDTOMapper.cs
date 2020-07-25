using CocktailMagician.Services.DTOs;
using CocktailMagician.Web.Models;

namespace CocktailMagician.Web.Mappers.Contracts
{
    public interface ICityDTOMapper
    {
        public CityViewModel MapToVMFromDTO(CityDTO cityDTO);
        public CityDTO MapToDTOFromVM(CityViewModel cityVM);
    }
}

using CocktailMagician.Services.DTOs;
using CocktailMagician.Web.Mappers.Contracts;
using CocktailMagician.Web.Models;
using System.Linq;

namespace CocktailMagician.Web.Mappers
{
    public class CityDTOMapper : ICityDTOMapper
    {
        public CityViewModel MapToVMFromDTO(CityDTO cityDTO)
        {
            CityViewModel cityVM = new CityViewModel
            {
                Id = cityDTO.Id,
                Name = cityDTO.Name,
                Bars = cityDTO.Bars
                            .Select(bar => new BarViewModel { Id = bar.Id, Name = bar.Name })
                            .ToList(),
            };

            return cityVM;
        }

        public CityDTO MapToDTOFromVM(CityViewModel cityVM)
        {
            CityDTO cityDTO = new CityDTO
            {
                Name = cityVM.Name,
                Bars = cityVM.Bars
                            .Select(bar => new BarDTO { Id = bar.Id, Name = bar.Name })
                            .ToList(),
            };

            return cityDTO;
        }
    }
}

using CocktailMagician.Services.DTOs;
using CocktailMagician.Web.Mappers.Contracts;
using CocktailMagician.Web.Models;
using System.Linq;

namespace CocktailMagician.Web.Mappers
{
    public class BarDTOMapper : IBarDTOMapper
    {
        public BarDTOMapper()
        {

        }
        public BarViewModel MapToVMFromDTO(BarDTO barDTO)
        {
            if (barDTO == null)
            {
                return null;
            }

            var barVM = new BarViewModel
            {
                Id = barDTO.Id,
                Name = barDTO.Name,
                AverageRating = barDTO.AverageRating,
                Address = barDTO.Address,
                Phone = barDTO.Phone,
                CityName = barDTO.CityName,
                Cocktails = barDTO.Cocktails.Select(c => new CocktailViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    AverageRating = c.AverageRating
                }).ToList(),
                ImageData = barDTO.ImageData,
                ImageSource = barDTO.ImageSource,
            };

            return barVM;
        }
        public BarDTO MapToDTOFromVM(BarViewModel barVM)
        {
            if (barVM == null)
            {
                return null;
            }
            var barDTO = new BarDTO
            {
                Name = barVM.Name,
                //CityName = barVM.CityName,
                CityId = barVM.CityId,
                Address = barVM.Address,
                Phone = barVM.Phone,
            };
            if (barVM.File != null)
            {
                barDTO.ImageData = barVM.ImageData;
            }

            return barDTO;
        }
    }
}

using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using System.Linq;

namespace CocktailMagician.Services.Mappers
{
    public class CityMapper : ICityMapper
    {
        public CityMapper()
        {

        }

        public CityDTO MapToCityDTO(City city)
        {
            CityDTO cityDTO = new CityDTO
            {
                Id = city.Id,
                Name = city.Name,
                Bars = city.Bars
                            .Where(b => !b.IsDeleted) //is it ok here?
                            .Select(bar => new BarDTO { Id = bar.Id, Name = bar.Name })
                            .ToList(),
            };

            return cityDTO;
        }
        public City MapToCity(CityDTO cityDTO)
        {
            City city = new City
            {
                Name = cityDTO.Name,
            };

            return city;
        }
    }
}

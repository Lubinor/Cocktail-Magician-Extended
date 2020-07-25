using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;

namespace CocktailMagician.Services.Mappers.Contracts
{
    public interface ICityMapper
    {
        public CityDTO MapToCityDTO(City city);
        public City MapToCity(CityDTO city);
    }
}

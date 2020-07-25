using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;

namespace CocktailMagician.Services.Mappers.Contracts
{
    public interface ICocktailMapper
    {
        public CocktailDTO MapToCocktailDTO(Cocktail cocktail);
        public Cocktail MapToCocktail(CocktailDTO cocktailDTO);

    }
}

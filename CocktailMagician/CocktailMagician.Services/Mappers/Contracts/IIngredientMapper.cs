using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;

namespace CocktailMagician.Services.Mappers.Contracts
{
    public interface IIngredientMapper
    {
        public IngredientDTO MapToIngredientDTO(Ingredient ingredient);
        public Ingredient MapToIngredient(IngredientDTO ingredientDTO);
    }
}

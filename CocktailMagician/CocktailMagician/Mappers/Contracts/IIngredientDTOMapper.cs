using CocktailMagician.Services.DTOs;
using CocktailMagician.Web.Models;

namespace CocktailMagician.Web.Mappers.Contracts
{
    public interface IIngredientDTOMapper
    {
        public IngredientViewModel MapToVMFromDTO(IngredientDTO ingredientDTO);
        public IngredientDTO MapToDTOFromVM(IngredientViewModel ingredientVM);
        public IngredientDTO MapToDTOFromVM(EditIngredientViewModel editIngredientVM);
    }
}

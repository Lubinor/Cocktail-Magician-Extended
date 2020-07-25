using CocktailMagician.Services.DTOs;
using CocktailMagician.Web.Mappers.Contracts;
using CocktailMagician.Web.Models;
using System.Linq;

namespace CocktailMagician.Web.Mappers
{
    public class IngredientDTOMapper : IIngredientDTOMapper
    {
        public IngredientDTOMapper()
        {

        }
        public IngredientViewModel MapToVMFromDTO(IngredientDTO ingredientDTO)
        {
            if (ingredientDTO == null)
            {
                return null;
            }
            var ingredientVM = new IngredientViewModel
            {
                Id = ingredientDTO.Id,
                Name = ingredientDTO.Name,
                Cocktails = ingredientDTO.CocktailDTOs.Select(c => new CocktailViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    AverageRating = c.AverageRating
                }).ToList(),
                ImageData = ingredientDTO.ImageData,
                ImageSource = ingredientDTO.ImageSource,
            };

            return ingredientVM;
        }
        public IngredientDTO MapToDTOFromVM(IngredientViewModel ingredientVM)
        {
            if (ingredientVM == null)
            {
                return null;
            }
            var ingredientDTO = new IngredientDTO
            {
                Name = ingredientVM.Name,
            };
            if (ingredientVM.File != null)
            {
                ingredientDTO.ImageData = ingredientVM.ImageData;
            }
            return ingredientDTO;
        }
        public IngredientDTO MapToDTOFromVM(EditIngredientViewModel editIngredientVM)
        {
            if (editIngredientVM == null)
            {
                return null;
            }
            var ingredientDTO = new IngredientDTO
            {
                Id = editIngredientVM.Id,
                Name = editIngredientVM.Name,
            };

            if (editIngredientVM.File != null)
            {
                ingredientDTO.ImageData = editIngredientVM.ImageData;
            }

            return ingredientDTO;
        }
    }
}

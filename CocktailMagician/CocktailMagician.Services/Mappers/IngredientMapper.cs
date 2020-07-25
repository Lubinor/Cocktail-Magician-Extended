using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using System;
using System.Linq;

namespace CocktailMagician.Services.Mappers
{
    public class IngredientMapper : IIngredientMapper
    {
        private readonly IDateTimeProvider datetimeProvider;

        public IngredientMapper(IDateTimeProvider datetimeProvider)
        {
            this.datetimeProvider = datetimeProvider ?? throw new ArgumentNullException(nameof(datetimeProvider));
        }

        public IngredientMapper()
        {
        }

        public IngredientDTO MapToIngredientDTO(Ingredient ingredient)
        {
            if (ingredient == null)
            {
                return null;
            }
            var ingredientDTO = new IngredientDTO
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                CocktailDTOs = ingredient.IngredientsCocktails.Select(c => new CocktailDTO
                {
                    Name = c.Cocktail.Name,
                    Id = c.Cocktail.Id
                }).ToList(),
                ImageData = ingredient.ImageData,
                ImageSource = ingredient.ImageSource

                //IsDeleted = ingredient.IsDeleted
            };
            return ingredientDTO;
        }
        public Ingredient MapToIngredient(IngredientDTO ingredientDTO)
        {
            if (ingredientDTO == null)
            {
                return null;
            }
            var ingredient = new Ingredient
            {
                Name = ingredientDTO.Name,
                ImageData = ingredientDTO.ImageData
                //CreatedOn = ingredientDTO.CreatedOn.HasValue ? ingredientDTO.CreatedOn : datetimeProvider.GetDateTime()
            };
            return ingredient;
        }
    }
}

using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using System;
using System.Linq;

namespace CocktailMagician.Services.Mappers
{
    public class CocktailMapper : ICocktailMapper
    {
        private readonly IDateTimeProvider datetimeProvider;

        public CocktailMapper(IDateTimeProvider datetimeProvider)
        {
            this.datetimeProvider = datetimeProvider ?? throw new ArgumentNullException(nameof(datetimeProvider));
        }
        public CocktailMapper()
        {

        }
        public CocktailDTO MapToCocktailDTO(Cocktail cocktail)
        {
            if (cocktail == null)
            {
                return null;
            }
            var cocktailDTO = new CocktailDTO
            {
                Id = cocktail.Id,
                Name = cocktail.Name,
                AverageRating = cocktail.AverageRating,
                Bars = cocktail.CocktailBars.Select(b => new BarDTO
                {
                    Id= b.Bar.Id,
                    Name = b.Bar.Name
                }).ToList(),
                Ingredients = cocktail.IngredientsCocktails.Select(i => new IngredientDTO
                {
                    Id = i.Ingredient.Id,
                    Name = i.Ingredient.Name
                }).ToList(),
                IsDeleted = cocktail.IsDeleted,
                ImageData = cocktail.ImageData,
                ImageSource = cocktail.ImageSource
            };
            return cocktailDTO;
        }
        public Cocktail MapToCocktail(CocktailDTO cocktailDTO)
        {
            if (cocktailDTO == null)
            {
                return null;
            }

            var cocktail = new Cocktail
            {
                Id = cocktailDTO.Id,
                Name = cocktailDTO.Name,
                AverageRating = cocktailDTO.AverageRating,
                IsDeleted = cocktailDTO.IsDeleted,
                IngredientsCocktails = cocktailDTO.Ingredients.Select(i => new IngredientsCocktails
                {
                    IngredientId = i.Id
                }).ToList(),
                ImageData = cocktailDTO.ImageData
                //CreatedOn = cocktailDTO.CreatedOn.HasValue ? cocktailDTO.CreatedOn : datetimeProvider.GetDateTime()
            };

            return cocktail;
        }
    }
}

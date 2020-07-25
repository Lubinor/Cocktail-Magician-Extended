using CocktailMagician.Services.DTOs;
using CocktailMagician.Web.Mappers.Contracts;
using CocktailMagician.Web.Models;
using System.Linq;

namespace CocktailMagician.Web.Mappers
{
    public class CocktailDTOMapper : ICocktailDTOMapper
    {
        public CocktailDTOMapper()
        {

        }
        public CocktailViewModel MapToVMFromDTO(CocktailDTO cocktailDTO)
        {
            if (cocktailDTO == null)
            {
                return null;
            }
            var cocktailVM = new CocktailViewModel
            {
                Id = cocktailDTO.Id,
                Name = cocktailDTO.Name,
                AverageRating = cocktailDTO.AverageRating,
                Bars = cocktailDTO.Bars.Select(b => new BarViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    //Address = b.Address,
                    CityName = b.CityName,
                    //Phone = b.Phone,
                    AverageRating = b.AverageRating
                }).ToList(),
                Ingredients = cocktailDTO.Ingredients.Select(i => new IngredientViewModel
                {
                    Id = i.Id,
                    Name = i.Name
                }).ToList(),
                ImageData = cocktailDTO.ImageData,
                ImageSource = cocktailDTO.ImageSource
            };
            return cocktailVM;
        }
        public CocktailDTO MapToDTOFromVM(CocktailViewModel cocktailVM)
        {
            if (cocktailVM == null)
            {
                return null;
            }
            var cocktailDTO = new CocktailDTO
            {
                Id = cocktailVM.Id,
                Name = cocktailVM.Name,
                AverageRating = cocktailVM.AverageRating,
                Ingredients = cocktailVM.Ingredients.Select(i => new IngredientDTO
                {
                    Id = i.Id,
                    Name = i.Name
                }).ToList(),
            };

            return cocktailDTO;
        }
        public CocktailDTO MapToDTOFromVM(CreateCocktailViewModel createCocktailVM)
        {
            if (createCocktailVM == null)
            {
                return null;
            }
            var cocktailDTO = new CocktailDTO
            {
                Name = createCocktailVM.Name,
                Ingredients = createCocktailVM.ContainedIngredients.Select(i => new IngredientDTO
                {
                    Id = i
                }).ToList(),
            };

            if (createCocktailVM.File != null)
            {
                cocktailDTO.ImageData = createCocktailVM.ImageData;
            }

            return cocktailDTO;
        }
        public CocktailDTO MapToDTOFromVM(EditCocktailViewModel editCocktailVM)
        {
            if (editCocktailVM == null)
            {
                return null;
            }
            var cocktailDTO = new CocktailDTO
            {
                Id = editCocktailVM.Id,
                Name = editCocktailVM.Name,
                Ingredients = editCocktailVM.ContainedIngredients.Select(i => new IngredientDTO
                {
                    Id = i
                }).ToList(),
                Bars = editCocktailVM.ContainedBars.Select(b => new BarDTO
                {
                    Id = b
                }).ToList(),
            };

            if (editCocktailVM.File != null)
            {
                cocktailDTO.ImageData = editCocktailVM.ImageData;
            }

            return cocktailDTO;
        }
    }
}

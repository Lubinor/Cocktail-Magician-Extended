using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace CocktailMagician.Tests.ServiceTests.DTOMappersTests
{
    [TestClass]
    public class IngredientMapper_Should
    {
        [TestMethod]
        public void CorretMapping_ToDTO()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(CorretMapping_ToDTO));
            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var ingredient = assertContext.Ingredients
                    .Include(ic => ic.IngredientsCocktails)
                        .ThenInclude(c => c.Cocktail)
                    .FirstOrDefault(i => i.Id == 2);

                var sut = new IngredientMapper();
                var result = sut.MapToIngredientDTO(ingredient);

                Assert.IsInstanceOfType(result, typeof(IngredientDTO));
                Assert.AreEqual(ingredient.Id, result.Id);
                Assert.AreEqual(ingredient.Name, result.Name);
                Assert.AreEqual(ingredient.IngredientsCocktails.Count, result.CocktailDTOs.Count);
            }
        }
        [TestMethod]
        public void CorrectMapping_ToIngredient()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(CorrectMapping_ToIngredient));

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var ingredientDTO = new IngredientDTO
                {
                    Name = "Coke"
                };
                
                var sut = new IngredientMapper();
                var result = sut.MapToIngredient(ingredientDTO);

                Assert.IsInstanceOfType(result, typeof(Ingredient));
                Assert.AreEqual(ingredientDTO.Name, result.Name);
            }

        }
    }
}

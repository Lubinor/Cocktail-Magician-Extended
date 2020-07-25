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
    public class CocktailMapper_Should
    {
        [TestMethod]
        public void CorrectMapping_ToCocktaiDTO()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(CorrectMapping_ToCocktaiDTO));
            Utils.GetInMemoryDataBase(options);

            //Act & Assert

            using (var assertCocntext = new CocktailMagicianContext(options))
            {
                var cocktail = assertCocntext.Cocktails
                    .Include(bar => bar.CocktailBars)
                        .ThenInclude(b => b.Bar)
                    .Include(ingr => ingr.IngredientsCocktails)
                        .ThenInclude(i => i.Ingredient)
                    .FirstOrDefault(x => x.Id == 1);

                var sut = new CocktailMapper();
                var result = sut.MapToCocktailDTO(cocktail);

                Assert.IsInstanceOfType(result, typeof(CocktailDTO));
                Assert.AreEqual(cocktail.Id, result.Id);
                Assert.AreEqual(cocktail.Name, result.Name);
                Assert.AreEqual(cocktail.AverageRating, result.AverageRating);
                Assert.AreEqual(cocktail.IsDeleted, result.IsDeleted);
                Assert.AreEqual(cocktail.IngredientsCocktails.Count, result.Ingredients.Count);
                Assert.AreEqual(cocktail.CocktailBars.Count, result.Bars.Count);
            }
        }
        [TestMethod]
        public void CorrectMapping_ToEntityModel()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(CorrectMapping_ToEntityModel));

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var cocktailDTO = new CocktailDTO
                {
                    Id = 5,
                    Name = "Cuba Libre",
                    AverageRating = 3.5,
                    IsDeleted = false
                };

                var sut = new CocktailMapper();
                var result = sut.MapToCocktail(cocktailDTO);

                Assert.IsInstanceOfType(result, typeof(Cocktail));
                Assert.AreEqual(cocktailDTO.Id, result.Id);
                Assert.AreEqual(cocktailDTO.Name, result.Name);
                Assert.AreEqual(cocktailDTO.AverageRating, result.AverageRating);
                Assert.AreEqual(cocktailDTO.IsDeleted, result.IsDeleted);
            }
        }
    }
}

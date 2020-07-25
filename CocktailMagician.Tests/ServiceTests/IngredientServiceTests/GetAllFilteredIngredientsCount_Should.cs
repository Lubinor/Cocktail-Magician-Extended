using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CocktailMagician.Tests.ServiceTests.IngredientServiceTests
{
    [TestClass]
    public class GetAllFilteredIngredientsCount_Should
    {
        [TestMethod]
        public void ReturnCorrectCountOfFilteredIngredients()
        {
            //Arrange
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIngMapper = new Mock<IIngredientMapper>();
            mockIngMapper.Setup(i => i.MapToIngredientDTO(It.IsAny<Ingredient>()))
                .Returns<Ingredient>(i => new IngredientDTO { Name = i.Name });
            var mockCocktailMapper = new Mock<ICocktailMapper>();
            var options = Utils.GetOptions(nameof(ReturnCorrectCountOfFilteredIngredients));
            
            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new IngredientService(mockDateTimeProvider.Object, mockIngMapper.Object,
                    mockCocktailMapper.Object, assertContext);
                var result = sut.GetAllFilteredIngredientsCount("o");

                Assert.AreEqual(2, result);
            }
        }
    }
}

using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.IngredientServiceTests
{
    [TestClass]
    public class GetAllIngredients_Should
    {
        [TestMethod]
        public async Task ReturnCorrectIngredientsAsync()
        {
            //Arrange
            var mockDatetimeProvider = new Mock<IDateTimeProvider>();
            var mockMapper = new Mock<IIngredientMapper>();
            mockMapper.Setup(i => i.MapToIngredientDTO(It.IsAny<Ingredient>()))
                .Returns<Ingredient>(i => new IngredientDTO { Id = i.Id, Name = i.Name });
            var mockCocktailMapper = new Mock<ICocktailMapper>();
            var options = Utils.GetOptions(nameof(ReturnCorrectIngredientsAsync));
            
            Utils.GetInMemoryDataBase(options);

            // Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new IngredientService(mockDatetimeProvider.Object,mockMapper.Object,
                    mockCocktailMapper.Object, assertContext);
                var result = (await sut.GetAllIngredientsAsync()).ToList();
                var ingredientsCount = assertContext.Ingredients.Count();

                Assert.AreEqual(ingredientsCount, result.Count);
            }
        }
    }
}

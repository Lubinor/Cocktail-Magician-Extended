using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.IngredientServiceTests
{
    [TestClass]
    public class FilteredIngredientsAsync_Shold
    {
        [TestMethod]
        public async Task FilterIngredientsBylName()
        {
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockCocktailMapper = new Mock<ICocktailMapper>();
            var mockIngMapper = new Mock<IIngredientMapper>();
            mockIngMapper.Setup(i => i.MapToIngredientDTO(It.IsAny<Ingredient>()))
                .Returns<Ingredient>(i => new IngredientDTO { Name = i.Name });
            var options = Utils.GetOptions(nameof(FilterIngredientsBylName));
            var filter = "in";
            var sort = "name";
            var direction = "asc";
            var expected = new List<IngredientDTO>
            {
                new IngredientDTO
                {
                    Name = "Dry Gin"
                },
                new IngredientDTO
                {
                    Name = "Mineral Water"
                }
            };

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new IngredientService(mockDateTimeProvider.Object, mockIngMapper.Object,
                    mockCocktailMapper.Object, assertContext);

                var result = await sut.ListAllIngredientsAsync(0, 10, filter, sort, direction);

                Assert.AreEqual(expected.Count, result.Count);
                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].Name, result[i].Name);
                }
            }
        }
    }
}

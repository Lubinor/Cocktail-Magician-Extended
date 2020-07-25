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
    public class SortIngredientsAsync_Should
    {
        [TestMethod]
        public async Task ReturnIngredientsSortedByNameAscending()
        {
            //Arrange
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockCocktailMapper = new Mock<ICocktailMapper>();
            var mockIngMapper = new Mock<IIngredientMapper>();
            mockIngMapper.Setup(i => i.MapToIngredientDTO(It.IsAny<Ingredient>()))
                .Returns<Ingredient>(i => new IngredientDTO { Name = i.Name });
            var options = Utils.GetOptions(nameof(ReturnIngredientsSortedByNameAscending));
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
                },
                new IngredientDTO
                {
                    Name = "Rum"
                },
                new IngredientDTO
                {
                    Name = "Soda"
                },
                new IngredientDTO
                {
                    Name = "Tonic"
                },
            };

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new IngredientService(mockDateTimeProvider.Object, mockIngMapper.Object,
                    mockCocktailMapper.Object , assertContext);
                
                var result = await sut.ListAllIngredientsAsync(0, 10, null, sort, direction);

                Assert.AreEqual(expected.Count, result.Count);
                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].Name, result[i].Name);
                }
            }
        }
        [TestMethod]
        public async Task ReturnIngredientsSortedByNameDescending()
        {
            //Arrange
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockCocktailMapper = new Mock<ICocktailMapper>();
            var mockIngMapper = new Mock<IIngredientMapper>();
            mockIngMapper.Setup(i => i.MapToIngredientDTO(It.IsAny<Ingredient>()))
                .Returns<Ingredient>(i => new IngredientDTO { Name = i.Name });
            var options = Utils.GetOptions(nameof(ReturnIngredientsSortedByNameDescending));
            var sort = "name";
            var direction = "desc";
            var expected = new List<IngredientDTO>
            {
                new IngredientDTO
                {
                    Name = "Tonic"
                },
                 new IngredientDTO
                {
                    Name = "Soda"
                },
                 new IngredientDTO
                {
                    Name = "Rum"
                },
                 new IngredientDTO
                {
                    Name = "Mineral Water"
                },
                new IngredientDTO
                {
                    Name = "Dry Gin"
                },
            };

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new IngredientService(mockDateTimeProvider.Object, mockIngMapper.Object,
                    mockCocktailMapper.Object, assertContext);

                var result = await sut.ListAllIngredientsAsync(0, 10, null, sort, direction);

                Assert.AreEqual(expected.Count, result.Count);
                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].Name, result[i].Name);
                }
            }
        }
    }
}

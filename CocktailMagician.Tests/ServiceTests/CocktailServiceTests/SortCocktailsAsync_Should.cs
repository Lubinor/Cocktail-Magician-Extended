using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.Contracts;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.CocktailServiceTests
{
    [TestClass]
    public class SortCocktailsAsync_Should
    {
        [TestMethod]
        public async Task ReturnCocktailsSortedByNameAscending()
        {
            //Arrange
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockCocktailMapper = new Mock<ICocktailMapper>();
            mockCocktailMapper.Setup(c => c.MapToCocktailDTO(It.IsAny<Cocktail>()))
                .Returns<Cocktail>(c => new CocktailDTO { Name = c.Name });
            var mockIngMapper = new Mock<IIngredientMapper>();
            var mockBarMapper = new Mock<IBarMapper>();
            var mockCocktailReviewService = new Mock<ICocktailReviewService>();
            var options = Utils.GetOptions(nameof(ReturnCocktailsSortedByNameAscending));
            var sort = "name";
            var direction = "desc";
            var expected = new List<CocktailDTO>
            {
                new CocktailDTO
                {
                    Name = "Mojito"
                },
                new CocktailDTO
                {
                    Name = "Gin Fizz"
                },
                new CocktailDTO
                {
                    Name = "Bozdugan"
                }
            };
            Utils.GetInMemoryDataBase(options);
            
            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CocktailService(mockDateTimeProvider.Object, mockCocktailMapper.Object,
                    mockIngMapper.Object,mockBarMapper.Object, assertContext, mockCocktailReviewService.Object);
                var result = await sut.ListAllCocktailsAsync(0,10,null,sort, direction);

                Assert.AreEqual(expected.Count, result.Count);
                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].Name, result[i].Name);
                }
            }
        }
        [TestMethod]
        public async Task ReturnCocktailsSortedByNameDescending()
        {
            //Arrange
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockCocktailMapper = new Mock<ICocktailMapper>();
            mockCocktailMapper.Setup(c => c.MapToCocktailDTO(It.IsAny<Cocktail>()))
                .Returns<Cocktail>(c => new CocktailDTO { Name = c.Name });
            var mockIngMapper = new Mock<IIngredientMapper>();
            var mockBarMapper = new Mock<IBarMapper>();
            var mockCocktailReviewService = new Mock<ICocktailReviewService>();
            var options = Utils.GetOptions(nameof(ReturnCocktailsSortedByNameDescending));
            var orderBy = "name";
            var direction = "asc";
            var expected = new List<CocktailDTO>
            {
                new CocktailDTO
                {
                    Name = "Bozdugan"
                },
                new CocktailDTO
                {
                    Name = "Gin Fizz"
                },
                new CocktailDTO
                {
                    Name = "Mojito"
                }
            };
            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CocktailService(mockDateTimeProvider.Object, mockCocktailMapper.Object,
                    mockIngMapper.Object, mockBarMapper.Object, assertContext, mockCocktailReviewService.Object);
                var result = await sut.ListAllCocktailsAsync(0,10,null,orderBy,direction);

                Assert.AreEqual(expected.Count, result.Count);
                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].Name, result[i].Name);
                }
            }
        }
        [TestMethod]
        public async Task OrderCocktailsByRatingDescending()
        {
            //Arrange
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockCocktailMapper = new Mock<ICocktailMapper>();
            mockCocktailMapper
                .Setup(c => c.MapToCocktailDTO(It.IsAny<Cocktail>()))
                .Returns<Cocktail>(c => new CocktailDTO { Name = c.Name, AverageRating = c.AverageRating });
            var mockIngMapper = new Mock<IIngredientMapper>();
            mockIngMapper
                .Setup(i => i.MapToIngredientDTO(It.IsAny<Ingredient>()))
                .Returns<Ingredient>(i => new IngredientDTO { Name = i.Name });
            var mockBarMapper = new Mock<IBarMapper>();
            var mockCocktailReviewService = new Mock<ICocktailReviewService>();
            var options = Utils.GetOptions(nameof(OrderCocktailsByRatingDescending));
            var orderBy = "AverageRating";
            var direction = "desc";
            var expected = new List<CocktailDTO>
            {
                new CocktailDTO
                {
                    Name = "Gin Fizz",
                    AverageRating = 4.9
                },
                new CocktailDTO
                {
                    Name = "Mojito",
                    AverageRating = 4.5,
                },
                new CocktailDTO
                {
                    Name = "Bozdugan",
                    AverageRating = 3.8,
                }
            };
            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CocktailService(mockDateTimeProvider.Object, mockCocktailMapper.Object,
                    mockIngMapper.Object, mockBarMapper.Object, assertContext, mockCocktailReviewService.Object);
                var result = await sut.ListAllCocktailsAsync(0, 10, null, orderBy, direction);

                Assert.AreEqual(expected.Count, result.Count);

                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].Name, result[i].Name);
                }
            }
        }
        [TestMethod]
        public async Task OrderCocktailsByRatingAscending()
        {
            //Arrange
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockCocktailMapper = new Mock<ICocktailMapper>();
            mockCocktailMapper
                .Setup(c => c.MapToCocktailDTO(It.IsAny<Cocktail>()))
                .Returns<Cocktail>(c => new CocktailDTO { Name = c.Name, AverageRating = c.AverageRating });
            var mockIngMapper = new Mock<IIngredientMapper>();
            mockIngMapper
                .Setup(i => i.MapToIngredientDTO(It.IsAny<Ingredient>()))
                .Returns<Ingredient>(i => new IngredientDTO { Name = i.Name });
            var mockBarMapper = new Mock<IBarMapper>();
            var mockCocktailReviewService = new Mock<ICocktailReviewService>();
            var options = Utils.GetOptions(nameof(OrderCocktailsByRatingAscending));
            var orderBy = "AverageRating";
            var direction = "asc";
            var expected = new List<CocktailDTO>
            {
                new CocktailDTO
                {
                    Name = "Bozdugan",
                    AverageRating = 3.8,
                },
                new CocktailDTO
                {
                    Name = "Mojito",
                    AverageRating = 4.5,
                },
                new CocktailDTO
                {
                    Name = "Gin Fizz",
                    AverageRating = 4.9
                }
            };
            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CocktailService(mockDateTimeProvider.Object, mockCocktailMapper.Object,
                    mockIngMapper.Object, mockBarMapper.Object, assertContext, mockCocktailReviewService.Object);
                var result = await sut.ListAllCocktailsAsync(0, 10, null, orderBy, direction);

                Assert.AreEqual(expected.Count, result.Count);

                for (int i = 0; i < expected.Count; i++)
                {
                    Assert.AreEqual(expected[i].Name, result[i].Name);
                }
            }
        }
    }
}

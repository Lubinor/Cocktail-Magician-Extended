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
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.CocktailServiceTests
{
    [TestClass]
    public class GetAllCocktailsAsync_Should
    {
        [TestMethod]
        public async Task ReturnCorrectCocktails()
        {
            //Arrange
            var mockDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockCocktailMapper = new Mock<ICocktailMapper>();
            mockCocktailMapper.Setup(c => c.MapToCocktailDTO(It.IsAny<Cocktail>()))
                .Returns<Cocktail>(c => new CocktailDTO { Id = c.Id, Name = c.Name, AverageRating = c.AverageRating });
            var mockIngMapper = new Mock<IIngredientMapper>();
            var mockBarMapper = new Mock<IBarMapper>();
            var mockCocktailReviewService = new Mock<ICocktailReviewService>();
            var options = Utils.GetOptions(nameof(ReturnCorrectCocktails));
           
            Utils.GetInMemoryDataBase(options);
            
            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CocktailService(mockDateTimeProvider.Object, mockCocktailMapper.Object,
                    mockIngMapper.Object, mockBarMapper.Object, assertContext, mockCocktailReviewService.Object);
                var result = (await sut.GetAllCocktailssAsync()).ToList();
                var cocktailsCount = assertContext.Cocktails.Count();

                Assert.AreEqual(cocktailsCount, result.Count);
            }
        }
    }
}

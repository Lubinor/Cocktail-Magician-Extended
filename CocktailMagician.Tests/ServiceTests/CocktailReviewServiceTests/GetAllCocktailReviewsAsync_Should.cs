using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.CocktailReviewServiceTests
{
    [TestClass]
    public class GetAllCocktailReviewsAsync_Should
    {
        [TestMethod]
        public async Task ReturnEmpty_IfNoCocktailReviews()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockICocktailReviewMapper = new Mock<ICocktailReviewMapper>();

            var options = Utils.GetOptions(nameof(ReturnEmpty_IfNoCocktailReviews));

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CocktailReviewService(mockIDateTimeProvider.Object, assertContext, mockICocktailReviewMapper.Object);

                var result = await sut.GetAllCocktailReviewsAsync(1);

                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod]
        public async Task Return_IfCocktailReviewDTOParamsAreValid()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockICocktailReviewMapper = new Mock<ICocktailReviewMapper>();

            var options = Utils.GetOptions(nameof(Return_IfCocktailReviewDTOParamsAreValid));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CocktailReviewService(mockIDateTimeProvider.Object, assertContext, mockICocktailReviewMapper.Object);

                var result = await sut.GetAllCocktailReviewsAsync(1);

                Assert.AreEqual(2, result.Count);
            }
        }
    }
}

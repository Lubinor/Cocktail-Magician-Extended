using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.BarReviewServiceTests
{
    [TestClass]
    public class GetAllBarReviewsAsync_Should
    {
        [TestMethod]
        public async Task ReturnEmpty_IfNoBarReviews()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIBarReviewMapper = new Mock<IBarReviewMapper>();

            var options = Utils.GetOptions(nameof(ReturnEmpty_IfNoBarReviews));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new BarReviewService(mockIDateTimeProvider.Object, assertContext, mockIBarReviewMapper.Object);

                var result = await sut.GetAllBarReviewsAsync(1);

                Assert.AreEqual(2, result.Count);
            }
        }

        [TestMethod]
        public async Task Return_IfParamsAreValid()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIBarReviewMapper = new Mock<IBarReviewMapper>();

            var options = Utils.GetOptions(nameof(Return_IfParamsAreValid));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new BarReviewService(mockIDateTimeProvider.Object, assertContext, mockIBarReviewMapper.Object);

                var result = await sut.GetAllBarReviewsAsync(1);

                Assert.AreEqual(2, result.Count);
            }
        }
    }
}

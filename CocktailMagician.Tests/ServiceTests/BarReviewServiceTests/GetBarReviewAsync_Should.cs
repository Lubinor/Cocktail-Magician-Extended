using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.BarReviewServiceTests
{
    [TestClass]
    public class GetBarReviewAsync_Should
    {
        [TestMethod]
        public async Task ReturnNull_IfNoBarReview()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIBarReviewMapper = new Mock<IBarReviewMapper>();

            var options = Utils.GetOptions(nameof(ReturnNull_IfNoBarReview));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new BarReviewService(mockIDateTimeProvider.Object, assertContext, mockIBarReviewMapper.Object);

                var result = await sut.GetBarReviewAsync(1, 1);

                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public async Task ReturnBReviewDTO_IfParamsAreValid()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIBarReviewMapper = new Mock<IBarReviewMapper>();
            mockIBarReviewMapper
                .Setup(r => r.MapToBarReviewDTO(It.IsAny<BarsUsersReviews>()))
                .Returns<BarsUsersReviews>(r => new BarReviewDTO { BarId = r.BarId, AuthorId = r.UserId, Comment = r.Comment });

            var options = Utils.GetOptions(nameof(ReturnBReviewDTO_IfParamsAreValid));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new BarReviewService(mockIDateTimeProvider.Object, assertContext, mockIBarReviewMapper.Object);

                var result = await sut.GetBarReviewAsync(2, 2);

                Assert.AreEqual(2, result.BarId);
                Assert.AreEqual(2, result.AuthorId);
            }
        }
    }
}
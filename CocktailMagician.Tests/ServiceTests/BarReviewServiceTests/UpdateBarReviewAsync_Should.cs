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
    public class UpdateBarReviewAsync_Should
    {
        [TestMethod]
        public async Task ReturnNull_IfNoUpdatedBReviewDTO()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIBarReviewMapper = new Mock<IBarReviewMapper>();
            mockIBarReviewMapper
                .Setup(r => r.MapToBarReview(It.IsAny<BarReviewDTO>()))
                .Returns<BarReviewDTO>(r => new BarsUsersReviews
                {
                    BarId = r.BarId,
                    UserId = r.AuthorId,
                    Comment = r.Comment
                });

            mockIBarReviewMapper
                .Setup(r => r.MapToBarReviewDTO(It.IsAny<BarsUsersReviews>()))
                .Returns<BarsUsersReviews>(r => new BarReviewDTO
                {
                    BarId = r.BarId,
                    AuthorId = r.UserId,
                    Comment = r.Comment
                });

            var options = Utils.GetOptions(nameof(ReturnNull_IfNoUpdatedBReviewDTO));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new BarReviewService(mockIDateTimeProvider.Object, assertContext, mockIBarReviewMapper.Object);

                var result = await sut.UpdateBarReviewAsync(1, 1, null);

                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public async Task ReturnUpdatedBReview_IfParamsAreValid()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIBarReviewMapper = new Mock<IBarReviewMapper>();
            mockIBarReviewMapper
                .Setup(r => r.MapToBarReview(It.IsAny<BarReviewDTO>()))
                .Returns<BarReviewDTO>(r => new BarsUsersReviews
                {
                    BarId = r.BarId,
                    UserId = r.AuthorId,
                    Comment = r.Comment,
                    Rating = r.Rating
                });

            mockIBarReviewMapper
                .Setup(r => r.MapToBarReviewDTO(It.IsAny<BarsUsersReviews>()))
                .Returns<BarsUsersReviews>(r => new BarReviewDTO
                {
                    BarId = r.BarId,
                    AuthorId = r.UserId,
                    Comment = r.Comment,
                    Rating = r.Rating
                });


            var options = Utils.GetOptions(nameof(ReturnUpdatedBReview_IfParamsAreValid));

            var updatedReview = new BarReviewDTO
            {
                BarId = 2,
                AuthorId = 2,
                Comment = "Worst!",
                Rating = 1
            };

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new BarReviewService(mockIDateTimeProvider.Object, assertContext, mockIBarReviewMapper.Object);

                var result = await sut.UpdateBarReviewAsync(2, 2, updatedReview);

                Assert.AreEqual("Worst!", result.Comment);
                Assert.AreEqual(1, result.Rating);
                Assert.AreEqual(2, result.AuthorId);
                Assert.AreEqual(2, result.BarId);
            }
        }
    }
}
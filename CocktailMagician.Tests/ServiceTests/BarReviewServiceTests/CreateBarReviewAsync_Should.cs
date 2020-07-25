using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.BarReviewServiceTests
{
    [TestClass]
    public class CreateBarReviewAsync_Should
    {
        [TestMethod]
        public async Task ReturnNull_IfNoBarReviewDTO()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIBarReviewMapper = new Mock<IBarReviewMapper>();

            var options = Utils.GetOptions(nameof(ReturnNull_IfNoBarReviewDTO));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new BarReviewService(mockIDateTimeProvider.Object, assertContext, mockIBarReviewMapper.Object);

                var result = await sut.CreateBarReviewAsync(null);

                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public async Task ReturnReview_IfParamsAreValid()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIBarReviewMapper = new Mock<IBarReviewMapper>();
            mockIBarReviewMapper
                .Setup(r => r.MapToBarReview(It.IsAny<BarReviewDTO>()))
                .Returns<BarReviewDTO>(r => new BarsUsersReviews { BarId = r.BarId, UserId = r.AuthorId, Comment = r.Comment });

            mockIBarReviewMapper
                .Setup(r => r.MapToBarReviewDTO(It.IsAny<BarsUsersReviews>()))
                .Returns<BarsUsersReviews>(r => new BarReviewDTO { BarId = r.BarId, AuthorId = r.UserId, Comment = r.Comment });

            var options = Utils.GetOptions(nameof(ReturnReview_IfParamsAreValid));
           
            var reviewDTO = new BarReviewDTO
            {
                BarId = 3,
                AuthorId = 1,
                Comment = "The Beach is the best!"
            };

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new BarReviewService(mockIDateTimeProvider.Object, assertContext, mockIBarReviewMapper.Object);

                var result = await sut.CreateBarReviewAsync(reviewDTO);

                Assert.AreEqual(6, assertContext.BarsUsersReviews.Count());
                Assert.AreEqual(1, result.AuthorId);
                Assert.AreEqual(3, result.BarId);
            }
        }
    }
}
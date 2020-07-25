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

namespace CocktailMagician.Tests.ServiceTests.CocktailReviewServiceTests
{
    [TestClass]
    public class CreateCocktailReviewAsync_Should
    {
        [TestMethod]
        public async Task ReturnNull_IfNoCocktailReviewDTO()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockICocktailReviewMapper = new Mock<ICocktailReviewMapper>();

            var options = Utils.GetOptions(nameof(ReturnNull_IfNoCocktailReviewDTO));

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CocktailReviewService(mockIDateTimeProvider.Object, assertContext, mockICocktailReviewMapper.Object);

                var result = await sut.CreateCocktailReviewAsync(null);

                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public async Task ReturnCocktailReview_IfParamsAreValid()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockICocktailReviewMapper = new Mock<ICocktailReviewMapper>();
            mockICocktailReviewMapper
                .Setup(r => r.MapToCocktailReview(It.IsAny<CocktailReviewDTO>()))
                .Returns<CocktailReviewDTO>(r => new CocktailsUsersReviews 
                { CocktailId = r.CocktailId, UserId = r.AuthorId, Comment = r.Comment });

            mockICocktailReviewMapper
                .Setup(r => r.MapToCocktailReviewDTO(It.IsAny<CocktailsUsersReviews>()))
                .Returns<CocktailsUsersReviews>(r => new CocktailReviewDTO 
                { CocktailId = r.CocktailId, AuthorId = r.UserId, Comment = r.Comment });

            var options = Utils.GetOptions(nameof(ReturnCocktailReview_IfParamsAreValid));

            var reviewDTO = new CocktailReviewDTO
            {
                CocktailId = 3,
                AuthorId = 2,
                Comment = "Bozdugan is for tough man!"
            };

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CocktailReviewService(mockIDateTimeProvider.Object, assertContext, mockICocktailReviewMapper.Object);

                var result = await sut.CreateCocktailReviewAsync(reviewDTO);

                Assert.AreEqual(5, assertContext.CocktailsUsersReviews.Count());
                Assert.AreEqual(2, result.AuthorId);
                Assert.AreEqual(3, result.CocktailId);
            }
        }
    }
}
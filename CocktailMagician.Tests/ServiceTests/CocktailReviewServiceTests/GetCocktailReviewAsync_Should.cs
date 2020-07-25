using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.CocktailReviewServiceTests
{
    [TestClass]
    public class GetCocktailReviewAsync_Should
    {
        [TestMethod]
        public async Task ReturnNull_IfNoCocktailReview()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockICocktailReviewMapper = new Mock<ICocktailReviewMapper>();

            var options = Utils.GetOptions(nameof(ReturnNull_IfNoCocktailReview));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CocktailReviewService(mockIDateTimeProvider.Object, assertContext, mockICocktailReviewMapper.Object);

                var result = await sut.GetCocktailReviewAsync(1, 1);

                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public async Task ReturnCReviewDTO_IfParamsAreValid()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockICocktailReviewMapper = new Mock<ICocktailReviewMapper>();
            mockICocktailReviewMapper
                .Setup(r => r.MapToCocktailReviewDTO(It.IsAny<CocktailsUsersReviews>()))
                .Returns<CocktailsUsersReviews>(r => new CocktailReviewDTO 
                { CocktailId = r.CocktailId, AuthorId = r.UserId, Comment = r.Comment });

            var options = Utils.GetOptions(nameof(ReturnCReviewDTO_IfParamsAreValid));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CocktailReviewService(mockIDateTimeProvider.Object, assertContext, mockICocktailReviewMapper.Object);

                var result = await sut.GetCocktailReviewAsync(1, 2);

                Assert.AreEqual(1, result.CocktailId);
                Assert.AreEqual(2, result.AuthorId);
            }
        }
    }
}
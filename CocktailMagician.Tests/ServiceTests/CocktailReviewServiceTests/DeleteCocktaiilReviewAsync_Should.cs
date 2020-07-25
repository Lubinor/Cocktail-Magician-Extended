using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.CocktailReviewServiceTests
{
    [TestClass]
    public class DeleteCocktailReviewAsync_Should
    {
        [TestMethod]
        public async Task ReturnNull_IfCocktailReviewMissingOrDeleted()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockICocktailReviewMapper = new Mock<ICocktailReviewMapper>();

            var options = Utils.GetOptions(nameof(ReturnNull_IfCocktailReviewMissingOrDeleted));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CocktailReviewService(mockIDateTimeProvider.Object, assertContext, mockICocktailReviewMapper.Object);

                var result = await sut.DeleteCocktailReviewAsync(3,2);

                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public async Task ReturnTrue_IfCocktailReviewDeletedSuccesfully()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockICocktailReviewMapper = new Mock<ICocktailReviewMapper>();

            var options = Utils.GetOptions(nameof(ReturnTrue_IfCocktailReviewDeletedSuccesfully));

            var review = new CocktailsUsersReviews
            {
                CocktailId = 1,
                UserId = 2,
                Comment = "Top!",
                Rating = 5
            };

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CocktailReviewService(mockIDateTimeProvider.Object, assertContext, mockICocktailReviewMapper.Object);

                var result = await sut.DeleteCocktailReviewAsync(1, 2);
                var deletedReview = assertContext.CocktailsUsersReviews
                    .FirstOrDefault(r => r.CocktailId == 1 && r.UserId == 2);


                Assert.IsTrue(result);
                Assert.AreEqual(true, deletedReview.IsDeleted);
            }
        }
    }
}
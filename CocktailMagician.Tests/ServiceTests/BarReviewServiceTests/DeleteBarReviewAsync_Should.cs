using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.BarReviewServiceTests
{
    [TestClass]
    public class DeleteBarReviewAsync_Should
    {
        [TestMethod]
        public async Task ReturnNull_IfBarReviewMissingOrDeleted()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIBarReviewMapper = new Mock<IBarReviewMapper>();

            var options = Utils.GetOptions(nameof(ReturnNull_IfBarReviewMissingOrDeleted));
           
            Utils.GetInMemoryDataBase(options);
            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new BarReviewService(mockIDateTimeProvider.Object, assertContext, mockIBarReviewMapper.Object);

                var result = await sut.DeleteBarReviewAsync(3,1);

                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public async Task ReturnTrue_IfBarReviewDeletedSuccesfully()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIBarReviewMapper = new Mock<IBarReviewMapper>();

            var options = Utils.GetOptions(nameof(ReturnTrue_IfBarReviewDeletedSuccesfully));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new BarReviewService(mockIDateTimeProvider.Object, assertContext, mockIBarReviewMapper.Object);

                var result = await sut.DeleteBarReviewAsync(2, 2);
                var deletedReview = assertContext.BarsUsersReviews
                    .FirstOrDefault(r => r.BarId == 2 && r.UserId == 2);


                Assert.IsTrue(result);
                Assert.AreEqual(true, deletedReview.IsDeleted);
            }
        }
    }
}
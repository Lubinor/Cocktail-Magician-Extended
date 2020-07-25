using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.Contracts;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.BarServiceTests
{
    [TestClass]
    public class DeleteBarAsync_Should
    {
        [TestMethod]
        public async Task ReturnFalse_IfBarDoesNotExistOrIsDeleted()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIBarMapper = new Mock<IBarMapper>();
            var mockIBarReviewService = new Mock<IBarReviewService>();

            var options = Utils.GetOptions(nameof(ReturnFalse_IfBarDoesNotExistOrIsDeleted));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new BarService(mockIDateTimeProvider.Object, assertContext, mockIBarMapper.Object, mockIBarReviewService.Object);

                var result = await sut.DeleteBarAsync(4);

                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public async Task DeleteBar_IfParamsAreValid()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIBarReviewService = new Mock<IBarReviewService>();

            var mockIBarMapper = new Mock<IBarMapper>();
            mockIBarMapper
                .Setup(x => x.MapToBarDTO(It.IsAny<Bar>()))
                .Returns<Bar>(b => new BarDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    CityId = b.CityId,
                    Address = b.Address,
                    Phone = b.Phone,
                    AverageRating = b.AverageRating
                });

            var options = Utils.GetOptions(nameof(DeleteBar_IfParamsAreValid));

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new BarService(mockIDateTimeProvider.Object, assertContext, mockIBarMapper.Object, mockIBarReviewService.Object);

                var result = await sut.DeleteBarAsync(1);

                var expectedBar = await assertContext.Bars.FirstOrDefaultAsync(b => b.Id == 1);

                Assert.IsTrue(result);
                Assert.AreEqual(true, expectedBar.IsDeleted);
            }
        }
    }
}

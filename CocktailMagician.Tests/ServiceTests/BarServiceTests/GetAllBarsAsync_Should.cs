using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.Contracts;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.BarServiceTests
{
    [TestClass]
    public class GetAllBarsAsync_Should
    {
        [TestMethod]
        public async Task Return_IfNoBars()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIBarReviewService = new Mock<IBarReviewService>();
            var mockIBarMapper = new Mock<IBarMapper>();

            var options = Utils.GetOptions(nameof(Return_IfNoBars));

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new BarService(mockIDateTimeProvider.Object, assertContext, mockIBarMapper.Object, mockIBarReviewService.Object);

                var result = await sut.GetAllBarsAsync();

                Assert.AreEqual(0, result.Count);
            }
        }

        [TestMethod]
        public async Task Return_ProperBarCount()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIBarReviewService = new Mock<IBarReviewService>();
            var mockIBarMapper = new Mock<IBarMapper>();
            mockIBarMapper.Setup(b => b.MapToBarDTO(It.IsAny<Bar>()))
                .Returns<Bar>(b => new BarDTO { Name = b.Name });

            var options = Utils.GetOptions(nameof(Return_ProperBarCount));
 
            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new BarService(mockIDateTimeProvider.Object, assertContext, mockIBarMapper.Object, mockIBarReviewService.Object);

                var result = await sut.GetAllBarsAsync();
                int barsCount = assertContext.Bars.Count();

                Assert.AreEqual(barsCount, result.Count);
            }
        }
    }
}

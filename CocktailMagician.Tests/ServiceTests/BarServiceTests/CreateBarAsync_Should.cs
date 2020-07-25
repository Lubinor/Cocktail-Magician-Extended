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
    public class CreateBarAsync_Should
    {
        [TestMethod]
        public async Task Return_WhenCreateBarInputIsNull()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockIBarMapper = new Mock<IBarMapper>();
            var mockIBarReviewService = new Mock<IBarReviewService>();

            var options = Utils.GetOptions(nameof(Return_WhenCreateBarInputIsNull));

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new BarService(mockIDateTimeProvider.Object, assertContext, mockIBarMapper.Object, mockIBarReviewService.Object);

                var result = await sut.CreateBarAsync(null);

                Assert.IsNull(result);
            }
        }

        //[TestMethod]
        //public async Task ReturnBar_WhenParamsAreValid()
        //{
        //    //Arrange
        //    var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
        //    var mockIBarMapper = new Mock<IBarMapper>();
        //    var mockIBarReviewService = new Mock<IBarReviewService>();

        //    var options = Utils.GetOptions(nameof(ReturnBar_WhenParamsAreValid));

        //    mockIBarMapper
        //        .Setup(x => x.MapToBar(It.IsAny<BarDTO>()))
        //        .Returns<BarDTO>(b => new Bar
        //        {
        //            Name = b.Name,
        //            CityId = b.CityId,
        //            Address = b.Address,
        //            Phone = b.Phone,
        //            ImageData = b.ImageData,
        //            ImageSource = b.ImageSource
        //        });


        //    var barDTO = new BarDTO
        //        {
        //            Name = "The Bar",
        //            CityId = 2,
        //            Address = "New Address str",
        //            Phone = "0888 999 555",
        //            ImageData =  new byte[] { },
        //            ImageSource = "~/Testimage/pqt-ozer-premium.jpg"
        //    };

        //    Utils.GetInMemoryDataBase(options);

        //    //Act & Assert
        //    using (var assertContext = new CocktailMagicianContext(options))
        //    {
        //        var sut = new BarService(mockIDateTimeProvider.Object, assertContext, mockIBarMapper.Object, mockIBarReviewService.Object);

        //        var result = await sut.CreateBarAsync(barDTO);

        //        Assert.IsInstanceOfType(result, typeof(BarDTO));
        //        Assert.AreEqual(barDTO.Name, result.Name);
        //        Assert.AreEqual(barDTO.CityId, result.CityId);
        //        Assert.AreEqual(barDTO.Address, result.Address);
        //        Assert.AreEqual(barDTO.Phone, result.Phone);
        //    }
        //}
    }
}

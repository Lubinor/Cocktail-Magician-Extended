using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.CityServiceTests
{
    [TestClass]
    public class CreateCityAsync_Should
    {
        [TestMethod]
        public async Task ReturnNull_WhenInputIsNull()
        {
            //Arrange
            var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
            var mockICityMapper = new Mock<ICityMapper>();
            var mockIBarMapper = new Mock<IBarMapper>();

            var options = Utils.GetOptions(nameof(ReturnNull_WhenInputIsNull));

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new CityService(mockIDateTimeProvider.Object, assertContext, mockICityMapper.Object, mockIBarMapper.Object);

                var result = await sut.CreateCityAsync(null);

                Assert.IsNull(result);
            }
        }

        //[TestMethod]
        //public async Task ReturnCity_WhenParamsAreValid()
        //{
        //    //Arrange
        //    var mockIDateTimeProvider = new Mock<IDateTimeProvider>();
        //    var mockICityMapper = new Mock<ICityMapper>();
        //    mockICityMapper
        //    .Setup(x => x.MapToCityDTO(It.IsAny<City>()))
        //    .Returns<City>(c => new CityDTO { Name = c.Name });
        //    var mockIBarMapper = new Mock<IBarMapper>();

        //    var options = Utils.GetOptions(nameof(ReturnCity_WhenParamsAreValid));

        //    var cityDTO = new CityDTO
        //        {
        //            Name = "Burgas"
        //        };

        //    var now = new DateTime(2020, 6, 10, 1, 1, 1, DateTimeKind.Utc);
        //    mockIDateTimeProvider.Setup(x => x.GetDateTime()).Returns(now);

        //    Utils.GetInMemoryDataBase(options);

        //    //Act & Assert
        //    using (var assertContext = new CocktailMagicianContext(options))
        //    {
        //        var sut = new CityService(mockIDateTimeProvider.Object, assertContext, mockICityMapper.Object, 
        //            mockIBarMapper.Object);

        //        var result = await sut.CreateCityAsync(cityDTO);

        //        Assert.IsInstanceOfType(result,typeof(CityDTO));
        //    }
        //}
    }
}

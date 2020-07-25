using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CocktailMagician.Tests.ServiceTests.DTOMappersTests
{
    [TestClass]
    public class CityMapper_Should
    {
        [TestMethod]
        public void CorrectMapping_ToCityDTO()
        {
            //Arrange
            var city = new City
            {
                Id = 3,
                Name = "Ruse",
            };

            //Act & Assert
            var sut = new CityMapper();
            var result = sut.MapToCityDTO(city);

            Assert.IsInstanceOfType(result, typeof(CityDTO));
            Assert.AreEqual(city.Id, result.Id);
            Assert.AreEqual(city.Name, result.Name);

        }
        [TestMethod]
        public void CorrectMapping_ToCity()
        {
            //Arrange
            var cityDTO = new CityDTO
            {
                Id = 4,
                Name = "Pleven"
            };

            //Act & Assert
            var sut = new CityMapper();
            var result = sut.MapToCity(cityDTO);

            Assert.IsInstanceOfType(result, typeof(City));
            Assert.AreEqual(cityDTO.Name, result.Name);
        }
    }
}

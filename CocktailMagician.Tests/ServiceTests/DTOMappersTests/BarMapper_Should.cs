using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers;
using CocktailMagician.Services.Mappers.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace CocktailMagician.Tests.ServiceTests.DTOMappersTests
{
    [TestClass]
    public class BarMapper_Should
    {
        [TestMethod]
        public void CorrectMapping_ToBarDTO()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(CorrectMapping_ToBarDTO));

            var bar = new Bar
            {
                Id = 3,
                Name = "The Beach",
                CityId = 2,
                City = new City
                {
                    Id = 2,
                    Name = "Kaspichan"
                },
                Address = "Obikolna str.",
                Phone = "0888 777 444",
                AverageRating = 4.5,
                IsDeleted = false
            };

            //Act & Assert
            using (var assertCocntext = new CocktailMagicianContext(options))
            {
                var sut = new BarMapper();
                var result = sut.MapToBarDTO(bar);

                Assert.IsInstanceOfType(result, typeof(BarDTO));
                Assert.AreEqual(bar.Id, result.Id);
                Assert.AreEqual(bar.Name, result.Name);
                Assert.AreEqual(bar.CityId, result.CityId);
                Assert.AreEqual(bar.City.Name, result.CityName);
                Assert.AreEqual(bar.Address, result.Address);
                Assert.AreEqual(bar.Phone, result.Phone);
                Assert.AreEqual(bar.AverageRating, result.AverageRating);
                Assert.AreEqual(bar.IsDeleted, result.IsDeleted);
                Assert.AreEqual(bar.BarCocktails.Count, result.Cocktails.Count);
            }
        }
        [TestMethod]
        public void CorrectMapping_ToBar()
        {
            //Arrange
            var options = Utils.GetOptions(nameof(CorrectMapping_ToBar));

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var barDTO = new BarDTO
                {
                    Name = "Bilkova",
                    CityId = 1,
                    Address = "Shishman str.",
                    Phone = "0888 888 444",
                    AverageRating = 4,
                };

                var sut = new BarMapper();
                var result = sut.MapToBar(barDTO);

                Assert.IsInstanceOfType(result, typeof(Bar));
                Assert.AreEqual(barDTO.Name, result.Name);
                Assert.AreEqual(barDTO.CityId, result.CityId);
                Assert.AreEqual(barDTO.Address, result.Address);
                Assert.AreEqual(barDTO.Phone, result.Phone);
                Assert.AreEqual(barDTO.AverageRating, result.AverageRating);
            }
        }
    }
}

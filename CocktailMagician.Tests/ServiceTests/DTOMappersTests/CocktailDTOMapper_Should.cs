using CocktailMagician.Services.DTOs;
using CocktailMagician.Web.Mappers;
using CocktailMagician.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CocktailMagician.Tests.ServiceTests.DTOMappersTests
{
    [TestClass]
    public class CocktailDTOMapper_Should
    {
        [TestMethod]
        public void CorectMapping_ToCocktailDTO()
        {
            //Arrange

            var cocktailVM = new CocktailViewModel
            {
                Id = 1,
                Name = "Gin Fizz",
                AverageRating = 5.0,
                
            };

            //Act & Assert

            var sut = new CocktailDTOMapper();
            var result = sut.MapToDTOFromVM(cocktailVM);

            Assert.IsInstanceOfType(result, typeof(CocktailDTO));
            Assert.AreEqual(cocktailVM.Id, result.Id);
            Assert.AreEqual(cocktailVM.Name, result.Name);
            Assert.AreEqual(cocktailVM.AverageRating, result.AverageRating);
        }
        [TestMethod]
        public void CorrectMapping_ToCocktailViewModel()
        {
            //Arrange

            var cocktailDTO = new CocktailDTO
            {
                Id = 5,
                Name = "Cuba Libre",
                AverageRating = 3.5,
                IsDeleted = false
            };

            //Act & Assert
            var sut = new CocktailDTOMapper();
            var result = sut.MapToVMFromDTO(cocktailDTO);

            Assert.IsInstanceOfType(result, typeof(CocktailViewModel));
            Assert.AreEqual(cocktailDTO.Id, result.Id);
            Assert.AreEqual(cocktailDTO.Name, result.Name);
            Assert.AreEqual(cocktailDTO.AverageRating, result.AverageRating);
        }
    }
}

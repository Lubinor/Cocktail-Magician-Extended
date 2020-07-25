using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CocktailMagician.Tests.ServiceTests.DTOMappersTests
{
    [TestClass]
    public class UserMapper_Should
    {
        [TestMethod]
        public void CorrectReturnInstanceType_ToUserDTO()
        {
            //Arrange
            var sut = new UserMapper();

            var options = Utils.GetOptions(nameof(CorrectReturnInstanceType_ToUserDTO));

            var user = new User { Id = 1, UserName = "Tom", Email = "tom@tom.com", PhoneNumber = "0898 989898" };

            //Act
            var result = sut.MapToUserDTO(user);

            //Assert
            Assert.IsInstanceOfType(result, typeof(UserDTO));
        }

        [TestMethod]
        public void CorrectMapping_ToUserDTO()
        {
            //Arrange
            var sut = new UserMapper();

            var options = Utils.GetOptions(nameof(CorrectMapping_ToUserDTO));

            var user = new User { Id = 1, UserName = "Tom", Email = "tom@tom.com", PhoneNumber = "0898 989898" };

            //Act
            var result = sut.MapToUserDTO(user);

            //Assert
            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual(user.UserName, result.UserName);
            Assert.AreEqual(user.Email, result.Email);
            Assert.AreEqual(user.PhoneNumber, result.PhoneNumber);
        }

        [TestMethod]
        public void CorrectReturnInstanceType_ToUser()
        {
            //Arrange
            var sut = new UserMapper();

            var options = Utils.GetOptions(nameof(CorrectReturnInstanceType_ToUser));

            var userDTO = new UserDTO { Id = 1, UserName = "Tom", Email = "tom@tom.com", PhoneNumber = "0898 989898" };

            //Act
            var result = sut.MapToUser(userDTO);

            //Assert
            Assert.IsInstanceOfType(result, typeof(User));
        }

        [TestMethod]
        public void CorrectMapping_ToUser()
        {
            //Arrange
            var sut = new UserMapper();

            var options = Utils.GetOptions(nameof(CorrectMapping_ToUser));

            var userDTO = new UserDTO { Id = 1, UserName = "Tom", Email = "tom@tom.com", PhoneNumber = "0898 989898" };

            //Act
            var result = sut.MapToUser(userDTO);

            //Assert
            Assert.AreEqual(userDTO.Id, result.Id);
            Assert.AreEqual(userDTO.UserName, result.UserName);
            Assert.AreEqual(userDTO.Email, result.Email);
            Assert.AreEqual(userDTO.PhoneNumber, result.PhoneNumber);
        }
    }
}

using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.UserServiceTests
{
    [TestClass]
    public class UpdateUser_Should
    {
        [TestMethod]
        public async Task ReturnNull_NoUserToUpdate()
        {
            //Arrange
            var mockDatetimeProvider = new Mock<IDateTimeProvider>();
            var mockIUserMapper = new Mock<IUserMapper>();

            var options = Utils.GetOptions(nameof(ReturnNull_NoUserToUpdate));

            var userDTO = new UserDTO
            {
                UserName = "Robert",
                Email = "Robert@abv.bg",
                PhoneNumber = "0833 333 333"
            };

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new UserService(mockDatetimeProvider.Object, assertContext, mockIUserMapper.Object);
                var result = await sut.UpdateUserAsync(3, userDTO);

                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public async Task UpdateUser_WhenParamsAreValid()
        {
            //Arrange
            var mockDatetimeProvider = new Mock<IDateTimeProvider>();
            var mockIUserMapper = new Mock<IUserMapper>();
            mockIUserMapper
                .Setup(x => x.MapToUserDTO(It.IsAny<User>()))
                .Returns<User>(x => new UserDTO { Id = x.Id, UserName = x.UserName, Email = x.Email, PhoneNumber = x.PhoneNumber });

            mockIUserMapper
                .Setup(x => x.MapToUser(It.IsAny<UserDTO>()))
                .Returns<UserDTO>(x => new User { UserName = x.UserName, Email = x.Email, PhoneNumber = x.PhoneNumber });

            var options = Utils.GetOptions(nameof(UpdateUser_WhenParamsAreValid));
            
            var userDTO = new UserDTO
            {
                UserName = "Robert",
                Email = "Robert@abv.bg",
                PhoneNumber = "0833 333 333"
            };

            Utils.GetInMemoryDataBase(options);

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new UserService(mockDatetimeProvider.Object, assertContext, mockIUserMapper.Object);
                var result = await sut.UpdateUserAsync(1, userDTO);

                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(userDTO.UserName, result.UserName);
                Assert.AreEqual(userDTO.Email, result.Email);
                Assert.AreEqual(userDTO.PhoneNumber, result.PhoneNumber);
            }
        }
    }
}

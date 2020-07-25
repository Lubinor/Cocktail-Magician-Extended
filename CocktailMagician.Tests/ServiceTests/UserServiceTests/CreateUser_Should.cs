using CocktailMagician.Data;
using CocktailMagician.Models;
using CocktailMagician.Services;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace CocktailMagician.Tests.ServiceTests.UserServiceTests
{
    [TestClass]
    public class CreateUser_Should
    {
        [TestMethod]
        public async Task ReturnNull_IfNoUserDTO()
        {
            //Arrange
            var mockDatetimeProvider = new Mock<IDateTimeProvider>();
            var mockIUserMapper = new Mock<IUserMapper>();

            var options = Utils.GetOptions(nameof(ReturnNull_IfNoUserDTO));

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new UserService(mockDatetimeProvider.Object, assertContext, mockIUserMapper.Object);
                var result = await sut.CreateUserAsync(null);

                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public async Task CreateUser_WhenParamsAreValid()
        {
            //Arrange
            var mockDatetimeProvider = new Mock<IDateTimeProvider>();
            var mockIUserMapper = new Mock<IUserMapper>();
            mockIUserMapper
                .Setup(x => x.MapToUserDTO(It.IsAny<User>()))
                .Returns<User>(x => new UserDTO { UserName = x.UserName, Email = x.Email, PhoneNumber = x.PhoneNumber });

            mockIUserMapper
                .Setup(x => x.MapToUser(It.IsAny<UserDTO>()))
                .Returns<UserDTO>(x => new User { UserName = x.UserName, Email = x.Email, PhoneNumber = x.PhoneNumber });

            var options = Utils.GetOptions(nameof(CreateUser_WhenParamsAreValid));

            var userDTO = new UserDTO
            {
                UserName = "George",
                Email = "George@abv.bg",
                PhoneNumber = "0899 899 899"
            };

            //Act & Assert
            using (var assertContext = new CocktailMagicianContext(options))
            {
                var sut = new UserService(mockDatetimeProvider.Object, assertContext, mockIUserMapper.Object);
                var result = await sut.CreateUserAsync(userDTO);

                Assert.AreEqual(1, assertContext.Users.Count());
            }
        }
    }
}

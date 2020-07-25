using CocktailMagician.Models;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;

namespace CocktailMagician.Services.Mappers
{
    public class UserMapper : IUserMapper
    {
        public UserDTO MapToUserDTO(User user)
        {
            var userDTO = new UserDTO 
            {
                Id = user.Id,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };

            return userDTO;
        }

        public User MapToUser(UserDTO userDTO)
        {
            var user = new User
            {
                Id = userDTO.Id,
                UserName = userDTO.UserName,
                PhoneNumber = userDTO.PhoneNumber,
                Email = userDTO.Email
            };

            return user;
        }
    }
}

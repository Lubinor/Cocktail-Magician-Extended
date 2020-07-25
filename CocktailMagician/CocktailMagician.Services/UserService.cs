using CocktailMagician.Data;
using CocktailMagician.Services.Contracts;
using CocktailMagician.Services.DTOs;
using CocktailMagician.Services.Mappers.Contracts;
using CocktailMagician.Services.Providers.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Services
{
    public class UserService : IUserService
    {
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly CocktailMagicianContext context;
        private readonly IUserMapper userMapper;

        public UserService(IDateTimeProvider dateTimeProvider, CocktailMagicianContext context, IUserMapper userMapper)
        {
            this.dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
        }

        public async Task<ICollection<UserDTO>> GetAllUsersAsync()
        {
            var usersDTO = await this.context.Users
                .Where(user => !user.IsDeleted)
                .Select(user => userMapper.MapToUserDTO(user))
                .ToListAsync();

            return usersDTO;
        }

        public async Task<UserDTO> GetUserAsync(int id)
        {
            var user = await this.context.Users
                .FirstOrDefaultAsync(user => !user.IsDeleted && user.Id == id);

            if (user == null)
            {
                return null;
            }

            var userDTO = userMapper.MapToUserDTO(user);

            return userDTO;
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return null;
            }

            var user = userMapper.MapToUser(userDTO);
            user.CreatedOn = dateTimeProvider.GetDateTime();

            this.context.Users.Add(user);
            await this.context.SaveChangesAsync();

            var newUserDTO = userMapper.MapToUserDTO(user);

            return newUserDTO;
        }

        public async Task<UserDTO> UpdateUserAsync(int id, UserDTO userDTO)
        {
            var user = await this.context.Users
                .FirstOrDefaultAsync(user => !user.IsDeleted && user.Id == id);

            if (user == null)
            {
                return null;
            }

            user.UserName = userDTO.UserName;
            user.PhoneNumber = userDTO.PhoneNumber;
            user.Email = userDTO.Email;

            this.context.Users.Update(user);
            await this.context.SaveChangesAsync();

            var updatedUserDTO = userMapper.MapToUserDTO(user);

            return updatedUserDTO;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await this.context.Users
                .FirstOrDefaultAsync(user => !user.IsDeleted && user.Id == id);

            if (user == null)
            {
                return false;
            }

            user.IsDeleted = true;

            context.Users.Update(user);
            await context.SaveChangesAsync();

            return true;
        }
    }
}

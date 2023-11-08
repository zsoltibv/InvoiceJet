using FacturilaAPI.Config;
using FacturilaAPI.Models.Entity;
using FacturilaAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using FacturilaAPI.Exceptions;

namespace FacturilaAPI.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly FacturilaDbContext _dbContext;
        public AuthService(FacturilaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> RegisterUser(UserRegisterDto userDto)
        {
            User user = new User();

            user.Email = userDto.Email;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;

            await _dbContext.User.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> LoginUser(UserLoginDto userDto)
        {
            User user = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if(user.Email != userDto.Email)
            {
                throw new UserNotFoundException(userDto.Email);
            }

            if(!BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
            {
                throw new Exception("Password is wrong");
            }

            return user;
        }
    }
}

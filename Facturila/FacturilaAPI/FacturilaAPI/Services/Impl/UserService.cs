using FacturilaAPI.Config;
using FacturilaAPI.Exceptions;
using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FacturilaAPI.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly FacturilaDbContext _dbContext;

        public UserService(FacturilaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserRegisterDto> GetUserByEmail([FromQuery] string email)
        {
            User user = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                throw new UserNotFoundException(email);
            }

            return new UserRegisterDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.PasswordHash
            };
        }
    }
}

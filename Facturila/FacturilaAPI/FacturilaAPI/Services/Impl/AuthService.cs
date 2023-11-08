using FacturilaAPI.Config;
using FacturilaAPI.Models.Entity;
using FacturilaAPI.Models.Dto;

namespace FacturilaAPI.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly FacturilaDbContext _dbContext;
        public AuthService(FacturilaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> RegisterUser(UserDto userDto)
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
    }
}

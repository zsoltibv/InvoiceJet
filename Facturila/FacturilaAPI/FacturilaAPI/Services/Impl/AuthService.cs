using FacturilaAPI.Config;
using FacturilaAPI.Models.Entity;
using FacturilaAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using FacturilaAPI.Exceptions;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace FacturilaAPI.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly FacturilaDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthService(FacturilaDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<User> RegisterUser(UserRegisterDto userDto)
        {
            User user = new User();
            user.Email = userDto.Email;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Role = "User";

            await _dbContext.User.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<string> LoginUser(UserLoginDto userDto)
        {
            User user = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (user.Email != userDto.Email)
            {
                throw new UserNotFoundException(userDto.Email);
            }

            if (!BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
            {
                throw new Exception("Password is wrong");
            }

            string token = CreateToken(user);

            return token;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: credentials
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}

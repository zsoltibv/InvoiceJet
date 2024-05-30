using InvoiceJetAPI.Config;
using InvoiceJetAPI.Models.Entity;
using InvoiceJetAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using InvoiceJetAPI.Exceptions;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using BC = BCrypt.Net.BCrypt;

namespace InvoiceJetAPI.Services.Impl
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
            var user = new User
            {
                Email = userDto.Email,
                PasswordHash = BC.HashPassword(userDto.Password),
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Role = "User"
            };

            await _dbContext.User.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<string> LoginUser(UserLoginDto userDto)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (user == null || user.Email != userDto.Email)
            {
                throw new UserNotFoundException(userDto.Email);
            }

            if (!BC.Verify(userDto.Password, user.PasswordHash))
            {
                throw new Exception("Password is incorrect.");
            }

            string token = CreateToken(user);
            return token;
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
                new Claim("email", user.Email),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: credentials
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}

using FacturilaAPI.Config;
using FacturilaAPI.Exceptions;
using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FacturilaAPI.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly FacturilaDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;

        public UserService(FacturilaDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
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
        public Guid? GetUserIdFromToken()
        {
            var userId = _httpContext.User.FindFirst("userId")?.Value;
            return new Guid(userId);
        }

        public async Task<int> GetUserFirmIdUsingTokenAsync()
        {
            var userFirmId = await _dbContext.User
             .Where(u => u.Id == GetUserIdFromToken())
             .Select(u => u.ActiveUserFirmId)
             .FirstOrDefaultAsync();

            return (int)userFirmId;
        }
    }
}

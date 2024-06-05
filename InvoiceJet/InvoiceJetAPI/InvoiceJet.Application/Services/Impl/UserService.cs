using AutoMapper;
using InvoiceJet.Domain.Exceptions;
using InvoiceJetAPI.Config;
using InvoiceJetAPI.Exceptions;
using InvoiceJetAPI.Models.Dto;
using InvoiceJet.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvoiceJetAPI.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly FacturilaDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public UserService(FacturilaDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
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
            var userIdValue = _httpContextAccessor.HttpContext?.User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdValue))
                return null;

            if (Guid.TryParse(userIdValue, out Guid userId))
                return userId;

            return null;
        }

        public async Task<int?> GetUserFirmIdUsingTokenAsync()
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return null;
            }

            var userFirmId = await _dbContext.User
                .Where(u => u.Id == userId)
                .Select(u => u.ActiveUserFirmId)
                .FirstOrDefaultAsync(); 

            return userFirmId; 
        }
    }
}

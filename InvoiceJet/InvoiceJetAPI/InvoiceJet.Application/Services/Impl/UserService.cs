using InvoiceJet.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InvoiceJet.Application.Services.Impl;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
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

        var userFirmId = await _unitOfWork.Users.Query()
            .Where(u => u.Id == userId)
            .Select(u => u.ActiveUserFirmId)
            .FirstOrDefaultAsync();

        return userFirmId;
    }
}
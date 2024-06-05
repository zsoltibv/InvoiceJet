using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace InvoiceJet.Application.Services.Impl;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetCurrentUserId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext.User.Identity.IsAuthenticated)
        {
            var userIdString = httpContext.User.FindFirst("userId")?.Value;
            
            if (Guid.TryParse(userIdString, out Guid userId))
            {
                return userId;
            }
        }
        return Guid.Empty;
    }
}
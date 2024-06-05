using InvoiceJet.Application.DTOs;

namespace InvoiceJet.Application.Services;

public interface IUserService
{
    Guid? GetUserIdFromToken();
    Task<int?> GetUserFirmIdUsingTokenAsync();
}
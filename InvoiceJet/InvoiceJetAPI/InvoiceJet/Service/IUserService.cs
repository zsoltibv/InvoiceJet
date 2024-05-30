using InvoiceJetAPI.Models.Dto;
using InvoiceJetAPI.Models.Entity;

namespace InvoiceJetAPI.Services
{
    public interface IUserService
    {
        Task<UserRegisterDto> GetUserByEmail(string email);
        Guid? GetUserIdFromToken();
        Task<int?> GetUserFirmIdUsingTokenAsync();
    }
}
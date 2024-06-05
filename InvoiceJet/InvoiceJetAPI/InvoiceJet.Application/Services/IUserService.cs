using InvoiceJetAPI.Models.Dto;

namespace InvoiceJetAPI.Services
{
    public interface IUserService
    {
        Task<UserRegisterDto> GetUserByEmail(string email);
        Guid? GetUserIdFromToken();
        Task<int?> GetUserFirmIdUsingTokenAsync();
    }
}
using InvoiceJetAPI.Models.Dto;
using InvoiceJetAPI.Models.Entity;

namespace InvoiceJetAPI.Services
{
    public interface IAuthService
    {
        Task<User> RegisterUser(UserRegisterDto userDto);
        Task<string> LoginUser(UserLoginDto userDto);
    }
}

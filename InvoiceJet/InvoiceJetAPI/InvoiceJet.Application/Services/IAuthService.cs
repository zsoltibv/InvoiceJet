using InvoiceJet.Domain.Models;
using InvoiceJetAPI.Models.Dto;

namespace InvoiceJetAPI.Services
{
    public interface IAuthService
    {
        Task<User> RegisterUser(UserRegisterDto userDto);
        Task<string> LoginUser(UserLoginDto userDto);
    }
}

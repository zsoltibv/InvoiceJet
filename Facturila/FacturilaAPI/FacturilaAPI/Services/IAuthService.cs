using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;

namespace FacturilaAPI.Services
{
    public interface IAuthService
    {
        Task<User> RegisterUser(UserRegisterDto userDto);
        Task<string> LoginUser(UserLoginDto userDto);
    }
}

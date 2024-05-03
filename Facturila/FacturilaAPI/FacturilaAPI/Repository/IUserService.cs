using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;

namespace FacturilaAPI.Services
{
    public interface IUserService
    {
        Task<UserRegisterDto> GetUserByEmail(string email);
        Guid? GetUserIdFromToken();
    }
}
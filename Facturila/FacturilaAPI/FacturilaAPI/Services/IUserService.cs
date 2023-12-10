using FacturilaAPI.Models.Entity;

namespace FacturilaAPI.Services
{
    public interface IUserService
    {
        Task<User> GetUserByEmail(string email);
    }
}
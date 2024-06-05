using InvoiceJet.Domain.Models;

namespace InvoiceJet.Domain.Interfaces.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    public Task<int> GetUserFirmIdAsync(Guid userId);
    public Task<UserFirm> GetUserFirmAsync(Guid userId);
}
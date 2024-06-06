using InvoiceJet.Domain.Interfaces.Repositories;
using InvoiceJet.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceJet.Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(InvoiceJetDbContext context) : base(context)
    {
    }

    public async Task<int?> GetUserFirmIdAsync(Guid userId)
    {
        var userFirmId = await _dbSet
            .Where(u => u.Id == userId)
                .Include(uf => uf.ActiveUserFirm)
            .Select(uf => uf.ActiveUserFirm.UserFirmId)
            .FirstOrDefaultAsync();

        return userFirmId;
    }

    public async Task<UserFirm?> GetUserFirmAsync(Guid userId)
    {
        var userFirm = await _dbSet
            .Where(u => u.Id == userId)
                .Include(uf => uf.ActiveUserFirm)
            .Select(uf => uf.ActiveUserFirm)
            .SingleOrDefaultAsync();

        return userFirm;
    }
}
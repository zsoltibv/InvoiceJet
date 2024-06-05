using InvoiceJet.Domain.Interfaces.Repositories;
using InvoiceJet.Domain.Models;

namespace InvoiceJet.Infrastructure.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(InvoiceJetDbContext context) : base(context)
    {
    }

    public Task<User?> FindByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }
}
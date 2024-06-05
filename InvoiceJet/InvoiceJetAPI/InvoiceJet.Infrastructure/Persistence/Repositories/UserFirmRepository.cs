using InvoiceJet.Domain.Interfaces.Repositories;
using InvoiceJet.Domain.Models;

namespace InvoiceJet.Infrastructure.Persistence.Repositories;

public class UserFirmRepository : GenericRepository<UserFirm>, IUserFirmRepository
{
    public UserFirmRepository(InvoiceJetDbContext context) : base(context)
    {
    }
}
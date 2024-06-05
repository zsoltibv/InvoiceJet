using InvoiceJet.Domain.Interfaces.Repositories;
using InvoiceJet.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceJet.Infrastructure.Persistence.Repositories;

public class BankAccountRepository : GenericRepository<BankAccount>, IBankAccountRepository
{
    public BankAccountRepository(InvoiceJetDbContext context) : base(context)
    {
    }

    public async Task<int> GetTotalBankAccountsAsync(int firmId)
    {
        return await _dbSet.Where(ba => ba.UserFirmId == firmId).CountAsync();
    }
}
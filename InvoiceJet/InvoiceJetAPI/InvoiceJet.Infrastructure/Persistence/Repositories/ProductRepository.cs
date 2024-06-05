using InvoiceJet.Domain.Interfaces.Repositories;
using InvoiceJet.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceJet.Infrastructure.Persistence.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(InvoiceJetDbContext context) : base(context)
    {
    }
    
    public async Task<int> GetTotalProductsAsync(int firmId)
    {
        return await _dbSet.Where(p => p.UserFirmId == firmId).CountAsync();
    }
}
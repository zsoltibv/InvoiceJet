using InvoiceJet.Domain.Interfaces.Repositories;
using InvoiceJet.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceJet.Infrastructure.Persistence.Repositories;

public class DocumentRepository : GenericRepository<Document>, IDocumentRepository
{
    public DocumentRepository(InvoiceJetDbContext context) : base(context)
    {
    }
    
    public async Task<int> GetTotalDocumentsAsync(int firmId)
    {
        return await _dbSet.CountAsync(d => d.UserFirmId == firmId);
    }
}
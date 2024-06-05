using InvoiceJet.Domain.Interfaces.Repositories;
using InvoiceJet.Domain.Models;

namespace InvoiceJet.Infrastructure.Persistence.Repositories;

public class DocumentProductRepository : GenericRepository<DocumentProduct>, IDocumentProductRepository
{
    public DocumentProductRepository(InvoiceJetDbContext context) : base(context)
    {
    }
}
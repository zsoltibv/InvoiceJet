using InvoiceJet.Domain.Interfaces.Repositories;
using InvoiceJet.Domain.Models;

namespace InvoiceJet.Infrastructure.Persistence.Repositories;

public class DocumentSeriesRepository : GenericRepository<DocumentSeries>, IDocumentSeriesRepository
{
    public DocumentSeriesRepository(InvoiceJetDbContext context) : base(context)
    {
    }
}
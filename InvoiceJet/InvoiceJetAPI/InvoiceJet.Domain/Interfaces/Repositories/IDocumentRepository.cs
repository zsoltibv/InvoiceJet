using InvoiceJet.Domain.Models;

namespace InvoiceJet.Domain.Interfaces.Repositories;

public interface IDocumentRepository : IGenericRepository<Document>
{
    Task<int> GetTotalDocumentsAsync(int firmId);
}
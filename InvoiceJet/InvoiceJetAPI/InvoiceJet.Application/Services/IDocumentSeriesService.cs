using InvoiceJet.Application.DTOs;

namespace InvoiceJet.Application.Services;

public interface IDocumentSeriesService
{
    Task<ICollection<DocumentSeriesDto>> GetAllDocumentSeriesForUserId(Guid userId);
}
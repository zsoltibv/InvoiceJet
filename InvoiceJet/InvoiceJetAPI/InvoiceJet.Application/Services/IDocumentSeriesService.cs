using InvoiceJet.Application.DTOs;

namespace InvoiceJet.Application.Services;

public interface IDocumentSeriesService
{
    Task<List<DocumentSeriesDto>> GetAllDocumentSeriesForUserId();
    Task AddInitialDocumentSeries(int userFirmId);
}
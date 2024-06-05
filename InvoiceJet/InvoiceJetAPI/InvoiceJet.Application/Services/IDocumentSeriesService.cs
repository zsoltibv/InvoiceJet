using InvoiceJetAPI.Models.Dto;

namespace InvoiceJetAPI.Services
{
    public interface IDocumentSeriesService
    {
        Task<ICollection<DocumentSeriesDto>> GetAllDocumentSeriesForUserId(Guid userId);
    }
}

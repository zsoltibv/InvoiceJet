using FacturilaAPI.Models.Dto;

namespace FacturilaAPI.Services
{
    public interface IDocumentSeriesService
    {
        Task<ICollection<DocumentSeriesDto>> GetAllDocumentSeriesForUserId(Guid userId);
    }
}

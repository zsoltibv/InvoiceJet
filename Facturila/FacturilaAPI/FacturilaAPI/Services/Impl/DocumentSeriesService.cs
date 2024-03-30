using FacturilaAPI.Models.Dto;

namespace FacturilaAPI.Services.Impl
{
    public class DocumentSeriesService : IDocumentSeriesService
    {
        public Task<ICollection<DocumentSeriesDto>> GetAllDocumentSeriesForUserId(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}

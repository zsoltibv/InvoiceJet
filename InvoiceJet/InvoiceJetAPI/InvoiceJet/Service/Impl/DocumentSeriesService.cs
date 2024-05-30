using AutoMapper;
using InvoiceJetAPI.Config;
using InvoiceJetAPI.Models.Dto;
using InvoiceJetAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace InvoiceJetAPI.Services.Impl
{
    public class DocumentSeriesService : IDocumentSeriesService
    {
        private readonly FacturilaDbContext _dbContext;
        private readonly IMapper _mapper;

        public DocumentSeriesService(FacturilaDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ICollection<DocumentSeriesDto>> GetAllDocumentSeriesForUserId(Guid userId)
        {
            var userWithFirm = await _dbContext.User
                .Where(u => u.Id == userId)
                .Include(u => u.ActiveUserFirm)
                .ThenInclude(ds => ds.DocumentSeries)!
                            .ThenInclude(dt => dt.DocumentType)
                .FirstOrDefaultAsync();

            if (userWithFirm?.ActiveUserFirm is null)
            {
                return new List<DocumentSeriesDto>();
            }

            return _mapper.Map<ICollection<DocumentSeries>, ICollection<DocumentSeriesDto>>(userWithFirm.ActiveUserFirm.DocumentSeries!);
        }
    }
}

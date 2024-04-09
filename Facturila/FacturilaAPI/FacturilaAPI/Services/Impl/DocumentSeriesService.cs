using AutoMapper;
using FacturilaAPI.Config;
using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace FacturilaAPI.Services.Impl
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
                        .ThenInclude(ds => ds.DocumentSeries)
                .FirstOrDefaultAsync();

            if (userWithFirm?.ActiveUserFirm == null)
            {
                return new List<DocumentSeriesDto>();
            }

            return _mapper.Map<ICollection<DocumentSeries>, ICollection<DocumentSeriesDto>>(userWithFirm.ActiveUserFirm.DocumentSeries);
        }
    }
}

using AutoMapper;
using FacturilaAPI.Config;
using FacturilaAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace FacturilaAPI.Services.Impl;

public class DocumentService : IDocumentService
{
    private readonly FacturilaDbContext _dbContext;
    private readonly IMapper _mapper;

    public DocumentService(FacturilaDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<DocumentAutofillDto> GetDocumentAutofillInfo(Guid userId, int documentTypeId)
    {
        var userFirmId = await _dbContext.User
            .Where(u => u.Id == userId)
            .Select(u => u.ActiveUserFirmId)
            .FirstOrDefaultAsync();

        if (userFirmId == null)
            return new DocumentAutofillDto();  

        var dto = new DocumentAutofillDto
        {
            Clients = await _dbContext.Firm
                .Where(f => f.UserFirms.Any(uf => uf.UserId == userId && uf.IsClient))
                .ToListAsync(),
            DocumentSeries = await _dbContext.DocumentSeries
                .Where(ds => ds.UserFirmId == userFirmId && ds.DocumentTypeId == documentTypeId)
                    .Include(ds => ds.DocumentType)
                .ToListAsync(),
            Products = await _dbContext.Product
                .Where(p => p.UserFirmId == userFirmId)
                .ToListAsync()
        };

        return dto;
    }
}
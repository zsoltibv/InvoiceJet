using AutoMapper;
using InvoiceJet.Application.DTOs;
using InvoiceJet.Domain.Interfaces;
using InvoiceJet.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceJet.Application.Services.Impl;

public class DocumentSeriesService : IDocumentSeriesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DocumentSeriesService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ICollection<DocumentSeriesDto>> GetAllDocumentSeriesForUserId(Guid userId)
    {
        var userWithFirm = await _unitOfWork.Users.Query()
            .Where(u => u.Id == userId)
            .Include(u => u.ActiveUserFirm)
            .ThenInclude(ds => ds.DocumentSeries)!
            .ThenInclude(dt => dt.DocumentType)
            .FirstOrDefaultAsync();

        if (userWithFirm?.ActiveUserFirm is null)
        {
            return new List<DocumentSeriesDto>();
        }

        return _mapper.Map<ICollection<DocumentSeries>, ICollection<DocumentSeriesDto>>(userWithFirm.ActiveUserFirm
            .DocumentSeries!);
    }
}
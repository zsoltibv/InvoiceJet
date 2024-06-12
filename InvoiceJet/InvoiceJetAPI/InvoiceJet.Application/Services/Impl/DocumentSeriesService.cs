using AutoMapper;
using InvoiceJet.Application.DTOs;
using InvoiceJet.Domain.Exceptions;
using InvoiceJet.Domain.Interfaces;
using InvoiceJet.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceJet.Application.Services.Impl;

public class DocumentSeriesService : IDocumentSeriesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public DocumentSeriesService(IMapper mapper, IUnitOfWork unitOfWork, IUserService userService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task<List<DocumentSeriesDto>> GetAllDocumentSeriesForUserId()
    {
        var userFirmId = await _unitOfWork.Users.GetUserFirmIdAsync(_userService.GetCurrentUserId());
        if (!userFirmId.HasValue)
        {
            throw new UserHasNoAssociatedFirmException();
        }

        var documentSeries = await _unitOfWork.DocumentSeries.GetAllDocumentSeriesForActiveUserFirm(_userService.GetCurrentUserId());
        return _mapper.Map<List<DocumentSeries>, List<DocumentSeriesDto>>(documentSeries);
    }

    public async Task AddInitialDocumentSeries(int userFirmId)
    {
        if(await _unitOfWork.DocumentSeries.Query().AnyAsync(d => d.UserFirmId == userFirmId))
        {
            return;
        }
        var documentSeries = new List<DocumentSeries>
        {
            new()
            {
                SeriesName = DateTime.Now.Year.ToString(),
                FirstNumber = 1,
                CurrentNumber = 1,
                IsDefault = true,
                DocumentType = await _unitOfWork.DocumentTypes.Query()
                    .Where(d => d.Name.Equals("Factura"))
                    .FirstOrDefaultAsync(),
                UserFirmId = userFirmId
            },
            new()
            {
                SeriesName = DateTime.Now.Year.ToString(),
                FirstNumber = 1,
                CurrentNumber = 1,
                IsDefault = true,
                DocumentType = await _unitOfWork.DocumentTypes.Query()
                    .Where(d => d.Name.Equals("Factura Storno"))
                    .FirstOrDefaultAsync(),
                UserFirmId = userFirmId
            },
            new()
            {
                SeriesName = DateTime.Now.Year.ToString(),
                FirstNumber = 1,
                CurrentNumber = 1,
                IsDefault = true,
                DocumentType = await _unitOfWork.DocumentTypes.Query()
                    .Where(d => d.Name.Equals("Proforma"))
                    .FirstOrDefaultAsync(),
                UserFirmId = userFirmId
            },
        };

        await _unitOfWork.DocumentSeries.AddRangeAsync(documentSeries);
        await _unitOfWork.CompleteAsync();
    }
}
using AutoMapper;
using InvoiceJet.Application.DTOs;
using InvoiceJet.Domain.Exceptions;
using InvoiceJet.Domain.Interfaces;
using InvoiceJet.Domain.Models;

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
}
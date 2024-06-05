using AutoMapper;
using InvoiceJet.Application.DTOs;
using InvoiceJet.Domain.Models;

namespace InvoiceJet.Application.MappingProfiles;

public class DocumentSeriesProfile : Profile
{
    public DocumentSeriesProfile()
    {
        CreateMap<DocumentSeries, DocumentSeriesDto>().ReverseMap();
    }
}
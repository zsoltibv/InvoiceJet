using AutoMapper;
using InvoiceJetAPI.Models.Dto;
using InvoiceJet.Domain.Models;

namespace InvoiceJetAPI.MappingProfiles
{
    public class DocumentSeriesProfile : Profile
    {
        public DocumentSeriesProfile()
        {
            CreateMap<DocumentSeries, DocumentSeriesDto>().ReverseMap();
        }
    }
}

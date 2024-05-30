using AutoMapper;
using InvoiceJetAPI.Models.Dto;
using InvoiceJetAPI.Models.Entity;

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

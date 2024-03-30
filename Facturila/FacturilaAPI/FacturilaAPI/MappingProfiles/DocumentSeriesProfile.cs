using AutoMapper;
using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;

namespace FacturilaAPI.MappingProfiles
{
    public class DocumentSeriesProfile : Profile
    {
        public DocumentSeriesProfile()
        {
            CreateMap<DocumentSeries, DocumentSeriesDto>().ReverseMap();
        }
    }
}

using AutoMapper;
using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;

namespace FacturilaAPI.MappingProfiles
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile() {
            CreateMap<DocumentRequestDTO, Document>()
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocumentSeries.DocumentType))
                .ForMember(dest => dest.DocumentProducts, opt => opt.MapFrom(src => src.Products))
                .ForMember(dest => dest.DocumentNumber, opt => opt.MapFrom(src => "INV" + src.DocumentSeries.SeriesName + src.DocumentSeries.FirstNumber.ToString()));
        }
    }
}

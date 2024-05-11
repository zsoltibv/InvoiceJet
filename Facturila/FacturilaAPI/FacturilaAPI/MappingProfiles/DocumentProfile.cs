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

            CreateMap<Document, DocumentTableRecordDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DocumentNumber, opt => opt.MapFrom(src => src.DocumentNumber))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client!.Name))
                .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.IssueDate))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.TotalPrice));
        }
    }
}

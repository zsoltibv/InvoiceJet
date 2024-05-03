using AutoMapper;
using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;

namespace FacturilaAPI.MappingProfiles
{
    public class DocumentProductProfile : Profile
    {
        public DocumentProductProfile() {
            CreateMap<DocumentProductRequestDTO, Product>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.UserFirm, opt => opt.Ignore())
                .ForMember(dest => dest.UserFirmId, opt => opt.Ignore());
        }
    }
}

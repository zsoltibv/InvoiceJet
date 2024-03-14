using AutoMapper;
using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Firm, FirmDto>(); 
    }
}
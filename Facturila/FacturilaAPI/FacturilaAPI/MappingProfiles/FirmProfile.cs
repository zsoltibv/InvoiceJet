using AutoMapper;
using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;

public class FirmProfile : Profile
{
    public FirmProfile()
    {
        CreateMap<Firm, FirmDto>().ReverseMap(); 
    }
}
using AutoMapper;
using InvoiceJetAPI.Models.Dto;
using InvoiceJetAPI.Models.Entity;

public class FirmProfile : Profile
{
    public FirmProfile()
    {
        CreateMap<Firm, FirmDto>().ReverseMap(); 
    }
}
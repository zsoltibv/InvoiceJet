using AutoMapper;
using InvoiceJetAPI.Models.Dto;
using InvoiceJet.Domain.Models;

public class FirmProfile : Profile
{
    public FirmProfile()
    {
        CreateMap<Firm, FirmDto>().ReverseMap(); 
    }
}
using AutoMapper;
using InvoiceJetAPI.Models.Dto;
using InvoiceJetAPI.Models.Entity;

public class BankAccountProfile : Profile
{
    public BankAccountProfile()
    {
        CreateMap<BankAccount, BankAccountDto>().ReverseMap();
    }
}
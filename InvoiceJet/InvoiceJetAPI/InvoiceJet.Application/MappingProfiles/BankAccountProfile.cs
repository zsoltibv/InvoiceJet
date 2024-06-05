using AutoMapper;
using InvoiceJetAPI.Models.Dto;
using InvoiceJet.Domain.Models;

public class BankAccountProfile : Profile
{
    public BankAccountProfile()
    {
        CreateMap<BankAccount, BankAccountDto>().ReverseMap();
    }
}
using AutoMapper;
using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;

public class BankAccountProfile : Profile
{
    public BankAccountProfile()
    {
        CreateMap<BankAccount, BankAccountDto>().ReverseMap();
    }
}
using AutoMapper;
using InvoiceJetAPI.Models.Dto;
using InvoiceJet.Domain.Models;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}

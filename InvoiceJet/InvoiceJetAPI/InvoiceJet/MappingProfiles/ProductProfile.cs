using AutoMapper;
using InvoiceJetAPI.Models.Dto;
using InvoiceJetAPI.Models.Entity;
using Microsoft.AspNetCore.Mvc;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}

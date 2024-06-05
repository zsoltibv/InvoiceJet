﻿using AutoMapper;
using InvoiceJet.Application.DTOs;
using InvoiceJet.Domain.Models;

namespace InvoiceJet.Application.MappingProfiles;

public class DocumentProfile : Profile
{
    public DocumentProfile()
    {
        CreateMap<Document, DocumentRequestDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Seller, opt => opt.MapFrom(src => src.UserFirm))
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client))
            .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.IssueDate))
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.DocumentProducts!.Select(dp =>
                new DocumentProductRequestDto
                {
                    Id = dp.Id,
                    Name = dp.Product!.Name,
                    UnitPrice = dp.UnitPrice,
                    TotalPrice = dp.TotalPrice,
                    ContainsTva = dp.Product.ContainsTva,
                    UnitOfMeasurement = dp.Product.UnitOfMeasurement!,
                    TvaValue = dp.Product != null ? dp.Product.TvaValue : 0,
                    Quantity = (int)dp.Quantity
                }).ToList()));

        CreateMap<Document, DocumentTableRecordDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.DocumentNumber, opt => opt.MapFrom(src => src.DocumentNumber))
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client!.Name))
            .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.IssueDate))
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate))
            .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.TotalPrice));
    }
}
using AutoMapper;
using InvoiceJetAPI.Models.Dto;
using InvoiceJetAPI.Models.Entity;

namespace InvoiceJetAPI.MappingProfiles
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile() {
            CreateMap<Document, DocumentRequestDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Seller, opt => opt.MapFrom(src => src.UserFirm))
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client))
            .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.IssueDate))
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate))
            .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.DocumentProducts.Select(dp => new DocumentProductRequestDTO
            {
                Id = dp.Id,
                Name = dp.Product.Name,
                UnitPrice = dp.UnitPrice,
                TotalPrice = dp.TotalPrice,
                ContainsTVA = dp.Product.ContainsTVA,
                UnitOfMeasurement =dp.Product.UnitOfMeasurement,
                TVAValue = dp.Product != null ? dp.Product.TVAValue : 0,
                Quantity = (int)dp.Quantity
            }).ToList()));

            CreateMap<Document, DocumentTableRecordDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DocumentNumber, opt => opt.MapFrom(src => src.DocumentNumber))
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client!.Name))
                .ForMember(dest => dest.IssueDate, opt => opt.MapFrom(src => src.IssueDate))
                .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate))
                .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.TotalPrice));
        }
    }
}

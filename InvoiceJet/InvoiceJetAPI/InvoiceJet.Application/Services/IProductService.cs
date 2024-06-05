using InvoiceJetAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceJetAPI.Services
{
    public interface IProductService {
        Task<ICollection<ProductDto>> GetAllProductsForUserId(Guid userId);
        Task<ProductDto> AddOrEditProduct(ProductDto productDto, Guid userId);
    }
}

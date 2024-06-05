using InvoiceJet.Application.DTOs;

namespace InvoiceJet.Application.Services;

public interface IProductService
{
    Task<ICollection<ProductDto>> GetAllProductsForUserId(Guid userId);
    Task<ProductDto> AddOrEditProduct(ProductDto productDto, Guid userId);
}
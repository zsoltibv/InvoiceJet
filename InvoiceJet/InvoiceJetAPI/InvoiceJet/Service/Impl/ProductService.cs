using AutoMapper;
using InvoiceJetAPI.Config;
using InvoiceJetAPI.Models.Dto;
using InvoiceJetAPI.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InvoiceJetAPI.Services.Impl
{
    public class ProductService : IProductService
    {
        private readonly FacturilaDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductService(FacturilaDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ProductDto> AddOrEditProduct(ProductDto productDto, Guid userId)
        {
            var activeUserFirm = await _dbContext.User
                   .Where(u => u.Id == userId)
                   .Include(u => u.ActiveUserFirm)
                        .ThenInclude(uf => uf.Products)
                   .Select(u => u.ActiveUserFirm)
                   .FirstOrDefaultAsync();

            if (activeUserFirm != null)
            {
                if (activeUserFirm.Products == null)
                {
                    activeUserFirm.Products = new List<Product>();
                }

                Product product;
                if (productDto.Id != 0 && activeUserFirm.Products.Any(p => p.Id == productDto.Id))
                {
                    product = activeUserFirm.Products.FirstOrDefault(p => p.Id == productDto.Id);
                    if (product != null)
                    {
                        _mapper.Map(productDto, product);
                    }
                }
                else
                {
                    product = _mapper.Map<Product>(productDto);
                    product.UserFirmId = activeUserFirm.UserFirmId;
                    activeUserFirm.Products.Add(product);
                }

                await _dbContext.SaveChangesAsync();
                productDto.Id = product.Id; 
            }

            return productDto;
        }


        public async Task<ICollection<ProductDto>> GetAllProductsForUserId(Guid userId)
        {
            var activeUserFirm = await _dbContext.User
                   .Where(u => u.Id == userId)
                   .Include(u => u.ActiveUserFirm)
                        .ThenInclude(uf => uf.Products)
                   .Select(u => u.ActiveUserFirm)
                   .FirstOrDefaultAsync();
            if (activeUserFirm == null)
            {
                return new List<ProductDto>();
            }
            var productDtos = _mapper.Map<ICollection<ProductDto>>(activeUserFirm.Products);
            return productDtos;
        }
    }
}

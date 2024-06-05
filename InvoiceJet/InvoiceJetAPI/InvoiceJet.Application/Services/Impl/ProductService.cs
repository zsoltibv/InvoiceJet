using AutoMapper;
using InvoiceJet.Application.DTOs;
using InvoiceJet.Domain.Interfaces;
using InvoiceJet.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceJet.Application.Services.Impl;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductDto> AddOrEditProduct(ProductDto productDto, Guid userId)
    {
        var activeUserFirm = await _unitOfWork.Users.Query()
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

            await _unitOfWork.CompleteAsync();
            productDto.Id = product.Id;
        }

        return productDto;
    }


    public async Task<ICollection<ProductDto>> GetAllProductsForUserId(Guid userId)
    {
        var activeUserFirm = await _unitOfWork.Users.Query()
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
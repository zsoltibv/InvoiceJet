using FacturilaAPI.Models.Dto;
using FacturilaAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FacturilaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAllProductsForUserId/{userId}")]
        public async Task<ActionResult<ICollection<ProductDto>>> GetAllProductsForUserId(Guid userId)
        {
            ICollection<ProductDto> productsDto = await _productService.GetAllProductsForUserId(userId);
            return Ok(productsDto);
        }

        [HttpPut("AddOrEditProduct/{userId}")]
        public async Task<ActionResult<ProductDto>> AddOrEditProduct(ProductDto product, Guid userId)
        {
            ProductDto productDto = await _productService.AddOrEditProduct(product, userId);
            return Ok(productDto);
        }
    }
}

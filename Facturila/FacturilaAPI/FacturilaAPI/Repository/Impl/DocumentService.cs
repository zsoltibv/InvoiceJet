using AutoMapper;
using FacturilaAPI.Config;
using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace FacturilaAPI.Services.Impl;

public class DocumentService : IDocumentService
{
    private readonly FacturilaDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public DocumentService(FacturilaDbContext dbContext, IMapper mapper, IUserService userService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<DocumentRequestDTO> AddOrEditDocument(DocumentRequestDTO documentRequestDTO)
    {
        var userFirmId = await _userService.GetUserFirmIdUsingTokenAsync();
        var documentProductsDto = documentRequestDTO.Products;
        var documentProducts = new List<DocumentProduct>();
        decimal totalInvoicePrice = 0;
        decimal totalInvoicePriceWithTVA = 0;

        Document document = new Document
        {
            Id = documentRequestDTO.Id,
            DocumentNumber = "INV" + documentRequestDTO.DocumentSeries.SeriesName + documentRequestDTO.DocumentSeries.FirstNumber,
            IssueDate = documentRequestDTO.IssueDate,
            DueDate = documentRequestDTO.DueDate,
            DocumentTypeId = documentRequestDTO.DocumentSeries.DocumentType?.Id,
            ClientId = documentRequestDTO.Client.Id,
            UserFirmId = userFirmId
        };

        _dbContext.Document.Add(document);
        await _dbContext.SaveChangesAsync();

        foreach (var productDto in documentProductsDto)
        {
            Product product;

            if (productDto.Id > 0)
            {
                product = await _dbContext.Product.FindAsync(productDto.Id);
                if (product == null)
                {
                    throw new Exception("Product not found.");
                }
            }
            else
            {
                product = _mapper.Map<Product>(productDto);
                product.UserFirmId = userFirmId;
                _dbContext.Product.Add(product);  // This will only be actually saved later
            }

            DocumentProduct documentProduct = new DocumentProduct
            {
                Quantity = productDto.Quantity,
                Product = product,
                DocumentId = document.Id,  // Now we have DocumentId available
                UnitPrice = productDto.UnitPrice,
                TotalPrice = productDto.TotalPrice,
            };

            totalInvoicePrice += productDto.UnitPrice * productDto.Quantity;
            totalInvoicePriceWithTVA += productDto.TotalPrice;

            _dbContext.DocumentProduct.Add(documentProduct);  // Add to DbContext
        }

        await _dbContext.SaveChangesAsync();  // Save everything at once
        return documentRequestDTO;
    }

    public async Task<DocumentAutofillDto> GetDocumentAutofillInfo(Guid userId, int documentTypeId)
    {
        var userFirmId = await _dbContext.User
            .Where(u => u.Id == userId)
            .Select(u => u.ActiveUserFirmId)
            .FirstOrDefaultAsync();

        if (userFirmId == null)
            return new DocumentAutofillDto();

        var dto = new DocumentAutofillDto
        {
            Clients = await _dbContext.Firm
                .Where(f => f.UserFirms.Any(uf => uf.UserId == userId && uf.IsClient))
                .ToListAsync(),
            DocumentSeries = await _dbContext.DocumentSeries
                .Where(ds => ds.UserFirmId == userFirmId && ds.DocumentTypeId == documentTypeId)
                    .Include(ds => ds.DocumentType)
                .ToListAsync(),
            Products = await _dbContext.Product
                .Where(p => p.UserFirmId == userFirmId)
                .ToListAsync()
        };

        return dto;
    }
}
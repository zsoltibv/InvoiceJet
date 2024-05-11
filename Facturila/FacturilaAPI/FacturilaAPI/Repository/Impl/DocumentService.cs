using AutoMapper;
using FacturilaAPI.Config;
using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;
using FacturilaAPI.Repository;
using FacturilaAPI.Repository.Impl;
using Microsoft.EntityFrameworkCore;

namespace FacturilaAPI.Services.Impl;

public class DocumentService : IDocumentService
{
    private readonly FacturilaDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IPdfGenerationService _pdfGenerationService;

    public DocumentService(FacturilaDbContext dbContext, IMapper mapper, IUserService userService, IPdfGenerationService pdfGenerationService)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _userService = userService;
        _pdfGenerationService = pdfGenerationService;
    }

    public async Task<DocumentRequestDTO> AddOrEditDocument(DocumentRequestDTO documentRequestDTO)
    {
        var userFirmId = await _userService.GetUserFirmIdUsingTokenAsync();
        var documentProductsDto = documentRequestDTO.Products;
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

    public async Task<DocumentRequestDTO> GeneratePdfDocument(DocumentRequestDTO documentRequestDTO)
    {
        var activeUserFirmId = await _userService.GetUserFirmIdUsingTokenAsync();
        var activeUserFirm = await _dbContext.UserFirm
            .Where(uf => uf.UserFirmId == activeUserFirmId)
            .Include(uf => uf.Firm)
            .FirstOrDefaultAsync();

        documentRequestDTO.Seller = _mapper.Map<FirmDto>(activeUserFirm?.Firm);

        //include invoice document class and generate pdf
        _pdfGenerationService.GenerateInvoicePdf(documentRequestDTO);
        

        return documentRequestDTO;
    }

    public async Task<byte[]> GetInvoicePdfStream(DocumentRequestDTO documentRequestDTO)
    {
        var activeUserFirmId = await _userService.GetUserFirmIdUsingTokenAsync();
        var activeUserFirm = await _dbContext.UserFirm
            .Where(uf => uf.UserFirmId == activeUserFirmId)
            .Include(uf => uf.Firm)
            .FirstOrDefaultAsync();

        documentRequestDTO.Seller = _mapper.Map<FirmDto>(activeUserFirm?.Firm);

        //include invoice document class and generate pdf
        return _pdfGenerationService.GetInvoicePdfStream(documentRequestDTO);
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
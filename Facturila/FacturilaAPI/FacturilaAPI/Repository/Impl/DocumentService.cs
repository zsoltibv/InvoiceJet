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

    private async Task UpdateDocumentProducts(int documentId, List<DocumentProductRequestDTO> documentProductsDto, int userFirmId)
    {
        decimal totalInvoicePrice = 0;
        decimal totalInvoicePriceWithTVA = 0;

        // Remove existing DocumentProducts if it's an edit operation
        var existingDocumentProducts = _dbContext.DocumentProduct.Where(dp => dp.DocumentId == documentId);
        _dbContext.DocumentProduct.RemoveRange(existingDocumentProducts);

        // Add new DocumentProducts
        foreach (var documentProductDto in documentProductsDto)
        {
            Product product;

            if (documentProductDto.Id > 0)
            {
                product = await _dbContext.Product.FirstOrDefaultAsync(product => product.Name == documentProductDto.Name);
                if (product == null)
                {
                    throw new Exception("Product not found.");
                }
            }
            else
            {
                product = _mapper.Map<Product>(documentProductDto);
                product.UserFirmId = userFirmId;
                _dbContext.Product.Add(product);  // This will only be actually saved later
            }

            DocumentProduct documentProduct = new DocumentProduct
            {
                Quantity = documentProductDto.Quantity,
                Product = product,
                DocumentId = documentId,  // Now we have DocumentId available
                UnitPrice = documentProductDto.UnitPrice,
                TotalPrice = documentProductDto.TotalPrice,
            };

            totalInvoicePrice += documentProductDto.UnitPrice * documentProductDto.Quantity;
            totalInvoicePriceWithTVA += documentProductDto.TotalPrice;

            _dbContext.DocumentProduct.Add(documentProduct);  // Add to DbContext
        }

        // Update the total prices for the document
        var document = await _dbContext.Document.FindAsync(documentId);
        if (document != null)
        {
            document.UnitPrice = totalInvoicePrice;
            document.TotalPrice = totalInvoicePriceWithTVA;
            _dbContext.Document.Update(document);
        }
    }

    public async Task<DocumentRequestDTO> AddDocument(DocumentRequestDTO documentRequestDTO)
    {
        var userFirmId = await _userService.GetUserFirmIdUsingTokenAsync();

        Document document = new Document
        {
            Id = documentRequestDTO.Id,
            DocumentNumber = "INV" + documentRequestDTO.DocumentSeries.SeriesName + documentRequestDTO.DocumentSeries.CurrentNumber.ToString("D4"),
            IssueDate = documentRequestDTO.IssueDate,
            DueDate = documentRequestDTO.DueDate,
            DocumentTypeId = documentRequestDTO.DocumentSeries.DocumentType?.Id,
            ClientId = documentRequestDTO.Client.Id,
            UserFirmId = userFirmId
        };

        _dbContext.Document.Add(document);
        await _dbContext.SaveChangesAsync();

        await UpdateDocumentProducts(document.Id, documentRequestDTO.Products, userFirmId);

        DocumentSeries docSeries = await _dbContext.DocumentSeries
            .Where(ds => ds.Id == documentRequestDTO.DocumentSeries.Id)
            .FirstOrDefaultAsync();

        docSeries.CurrentNumber++;
        _dbContext.DocumentSeries.Update(docSeries);

        await _dbContext.SaveChangesAsync();  // Save everything at once
        return documentRequestDTO;
    }

    public async Task<DocumentRequestDTO> EditDocument(DocumentRequestDTO documentRequestDTO)
    {
        var userFirmId = await _userService.GetUserFirmIdUsingTokenAsync();

        var document = await _dbContext.Document
            .FirstOrDefaultAsync(d => d.Id == documentRequestDTO.Id);

        if (document == null)
        {
            throw new Exception("Document not found.");
        }

        document.IssueDate = documentRequestDTO.IssueDate;
        document.DueDate = documentRequestDTO.DueDate;
        document.DocumentTypeId = documentRequestDTO.DocumentType?.Id;
        document.ClientId = documentRequestDTO.Client.Id;
        document.UserFirmId = userFirmId;

        _dbContext.Document.Update(document);

        await UpdateDocumentProducts(document.Id, documentRequestDTO.Products, userFirmId);

        await _dbContext.SaveChangesAsync();  
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

    public async Task<DocumentStreamDto> GetInvoicePdfStream(DocumentRequestDTO documentRequestDTO)
    {
        var activeUserFirmId = await _userService.GetUserFirmIdUsingTokenAsync();
        var activeUserFirm = await _dbContext.UserFirm
            .Where(uf => uf.UserFirmId == activeUserFirmId)
            .Include(uf => uf.Firm)
            .FirstOrDefaultAsync();

        documentRequestDTO.Seller = _mapper.Map<FirmDto>(activeUserFirm?.Firm);

        //include invoice document class and generate pdf
        return new DocumentStreamDto
        {
            DocumentNumber = documentRequestDTO.DocumentNumber ?? documentRequestDTO.DocumentSeries.CurrentNumber.ToString(),
            PdfContent = _pdfGenerationService.GetInvoicePdfStream(documentRequestDTO),
        };
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

    public async Task<List<DocumentTableRecordDTO>> GetDocumentTableRecords(int documentTypeId)
    {
        var activeUserFirmId = await _userService.GetUserFirmIdUsingTokenAsync();
        var activeUserFirm = await _dbContext.UserFirm
            .Where(uf => uf.UserFirmId == activeUserFirmId)
            .Include(uf => uf.Firm)
            .FirstOrDefaultAsync();

        if (activeUserFirm == null) return new List<DocumentTableRecordDTO>();

        var documents = await _dbContext.Document
            .Where(document => document.UserFirmId == activeUserFirmId && document.DocumentTypeId == documentTypeId)
                .Include(document => document.Client)
            .ToListAsync();

        return _mapper.Map<List<DocumentTableRecordDTO>>(documents);
    }

    public async Task<DocumentRequestDTO> GetDocumentById(int documentId)
    {
        var document = await _dbContext.Document
            .Where(d => d.Id == documentId)
                .Include(d => d.DocumentProducts)
                    .ThenInclude(dp => dp.Product)
                .Include(d => d.Client)
            .FirstOrDefaultAsync();

        return _mapper.Map<DocumentRequestDTO>(document);
    }
}
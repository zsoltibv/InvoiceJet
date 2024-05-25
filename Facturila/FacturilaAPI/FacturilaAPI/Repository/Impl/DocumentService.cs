using AutoMapper;
using FacturilaAPI.Config;
using FacturilaAPI.Models.Dto;
using FacturilaAPI.Models.Entity;
using FacturilaAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace FacturilaAPI.Services.Impl;

public class DocumentService : IDocumentService
{
    private readonly FacturilaDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IPdfGenerationService _pdfGenerationService;
    private readonly IServiceScopeFactory _scopeFactory;

    public DocumentService(FacturilaDbContext dbContext, IMapper mapper, IUserService userService, IPdfGenerationService pdfGenerationService,
        IServiceScopeFactory scopeFactory)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _userService = userService;
        _pdfGenerationService = pdfGenerationService;
        _scopeFactory = scopeFactory;
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
            DocumentStatusId = 1, 
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
        document.DocumentStatusId = documentRequestDTO.DocumentStatus?.Id;
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
            DocumentStatuses = await _dbContext.DocumentStatus.ToListAsync(),
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
                .Include(document => document.DocumentStatus)
            .ToListAsync();

        return _mapper.Map<List<DocumentTableRecordDTO>>(documents);
    }

    public async Task<DocumentRequestDTO> GetDocumentById(int documentId)
    {
        var document = await _dbContext.Document
            .Where(d => d.Id == documentId)
                .Include(d => d.DocumentStatus)
                .Include(d => d.DocumentProducts)
                    .ThenInclude(dp => dp.Product)
                .Include(d => d.Client)
            .FirstOrDefaultAsync();

        return _mapper.Map<DocumentRequestDTO>(document);
    }
    
    public async Task DeleteDocuments(int[] documentIds)
    {
        var documents = await _dbContext.Document
                .Include(dp => dp.DocumentProducts)
            .Where(d => documentIds.Contains(d.Id))
            .ToListAsync();
        
        _dbContext.DocumentProduct.RemoveRange(documents.SelectMany(d => d.DocumentProducts));
        _dbContext.Document.RemoveRange(documents);
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task<DashboardStatsDto> GetDashboardStats()
    {
        int activeUserFirmId = await _userService.GetUserFirmIdUsingTokenAsync();
        var activeUserFirm = await _dbContext.UserFirm
            .Where(uf => uf.UserFirmId == activeUserFirmId)
            .Include(uf => uf.User)
            .FirstOrDefaultAsync();
        
        if (activeUserFirm == null)
        {
            return new DashboardStatsDto(); 
        }

        var totalDocumentsTask = Task.Run(() => GetTotalDocuments(activeUserFirmId));
        var totalClientsTask = Task.Run(() => GetTotalClients(activeUserFirm.User.Id));
        var totalProductsTask = Task.Run(() => GetTotalProducts(activeUserFirmId));
        var totalBankAccountsTask = Task.Run(() => GetTotalBankAccounts(activeUserFirmId));
        var monthlyTotalsTask = Task.Run(() => GetMonthlyTotals(activeUserFirmId));

        await Task.WhenAll(totalDocumentsTask, totalClientsTask, totalProductsTask, totalBankAccountsTask, monthlyTotalsTask);

        return new DashboardStatsDto
        {
            TotalDocuments = await totalDocumentsTask,
            TotalClients = await totalClientsTask,
            TotalProducts = await totalProductsTask,
            TotalBankAccounts = await totalBankAccountsTask,
            MonthlyTotals = await monthlyTotalsTask
        };
    }
    
    private async Task<int> GetTotalDocuments(int firmId)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FacturilaDbContext>();
        return await dbContext.Document
            .Where(d => d.UserFirmId == firmId)
            .CountAsync();
    }
    
    private async Task<int> GetTotalClients(Guid userId)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FacturilaDbContext>();
        return await dbContext.Firm
            .CountAsync(f => f.UserFirms.Any(uf => uf.UserId == userId && uf.IsClient));
    }

    private async Task<int> GetTotalProducts(int firmId)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FacturilaDbContext>();
        return await dbContext.Product
            .Where(p => p.UserFirmId == firmId)
            .CountAsync();
    }

    private async Task<int> GetTotalBankAccounts(int firmId)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FacturilaDbContext>();
        return await dbContext.BankAccount
            .Where(ba => ba.UserFirmId == firmId)
            .CountAsync();
    }

    private async Task<List<MonthlyTotalDto>> GetMonthlyTotals(int firmId)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FacturilaDbContext>();
        return await dbContext.Document
            .Where(d => d.UserFirmId == firmId && d.IssueDate.Year == DateTime.Now.Year)
            .GroupBy(d => new { month = d.IssueDate.Month })
            .Select(group => new MonthlyTotalDto
            {
                Month = group.Key.month,
                InvoiceAmount = group.Sum(d => d.TotalPrice),
                IncomeAmount = group.Sum(d => d.DocumentStatusId == 2 ? d.TotalPrice : 0)
            })
            .OrderBy(x => x.Month)
            .ToListAsync();
    }
}
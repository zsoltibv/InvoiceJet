using AutoMapper;
using InvoiceJet.Application.DTOs;
using InvoiceJet.Domain.Interfaces;
using InvoiceJet.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceJet.Application.Services.Impl;

public class DocumentService : IDocumentService
{
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IPdfGenerationService _pdfGenerationService;
    private readonly IUnitOfWork _unitOfWork;

    public DocumentService(IMapper mapper, IPdfGenerationService pdfGenerationService, IUnitOfWork unitOfWork, IUserService userService)
    {
        _mapper = mapper;
        _pdfGenerationService = pdfGenerationService;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    private async Task UpdateDocumentProducts(int documentId, List<DocumentProductRequestDto> documentProductsDto,
        int userFirmId)
    {
        decimal totalInvoicePrice = 0;
        decimal totalInvoicePriceWithTVA = 0;

        // Remove existing DocumentProducts if it's an edit operation
        var existingDocumentProducts = _unitOfWork.DocumentProducts.Query().Where(dp => dp.DocumentId == documentId);
        await _unitOfWork.DocumentProducts.RemoveRangeAsync(existingDocumentProducts);

        // Add new DocumentProducts
        foreach (var documentProductDto in documentProductsDto)
        {
            Product product;

            if (documentProductDto.Id > 0)
            {
                product = await _unitOfWork.Products.Query()
                    .FirstOrDefaultAsync(product => product.Name == documentProductDto.Name);
                if (product == null)
                {
                    throw new Exception("Product not found.");
                }
            }
            else
            {
                product = _mapper.Map<Product>(documentProductDto);
                product.UserFirmId = userFirmId;
                await _unitOfWork.Products.AddAsync(product); // This will only be actually saved later
            }

            DocumentProduct documentProduct = new DocumentProduct
            {
                Quantity = documentProductDto.Quantity,
                Product = product,
                DocumentId = documentId, // Now we have DocumentId available
                UnitPrice = documentProductDto.UnitPrice,
                TotalPrice = documentProductDto.TotalPrice,
            };

            totalInvoicePrice += documentProductDto.UnitPrice * documentProductDto.Quantity;
            totalInvoicePriceWithTVA += documentProductDto.TotalPrice;

            await _unitOfWork.DocumentProducts.AddAsync(documentProduct); // Add to DbContext
        }

        // Update the total prices for the document
        var document = await _unitOfWork.Documents.GetByIdAsync(documentId);
        if (document != null)
        {
            document.UnitPrice = totalInvoicePrice;
            document.TotalPrice = totalInvoicePriceWithTVA;
            await _unitOfWork.Documents.UpdateAsync(document);
        }
    }

    public async Task<DocumentRequestDto> AddDocument(DocumentRequestDto documentRequestDto)
    {
        var userFirmId = await _unitOfWork.Users.GetUserFirmIdAsync(_userService.GetCurrentUserId());
        Document document = new Document
        {
            Id = documentRequestDto.Id,
            DocumentNumber = documentRequestDto.DocumentSeries.SeriesName +
                             documentRequestDto.DocumentSeries.CurrentNumber.ToString("D4"),
            IssueDate = documentRequestDto.IssueDate,
            DueDate = documentRequestDto.DueDate,
            DocumentTypeId = documentRequestDto.DocumentSeries.DocumentType?.Id,
            DocumentStatusId = 1,
            BankAccount = await _unitOfWork.BankAccounts.Query()
                .Where(ba => ba.UserFirmId == userFirmId && ba.IsActive)
                .FirstOrDefaultAsync(),
            ClientId = documentRequestDto.Client.Id,
            UserFirmId = userFirmId
        };

        await _unitOfWork.Documents.AddAsync(document);
        await _unitOfWork.CompleteAsync();

        await UpdateDocumentProducts(document.Id, documentRequestDto.Products, userFirmId);

        DocumentSeries docSeries = await _unitOfWork.DocumentSeries.Query()
            .Where(ds => ds.Id == documentRequestDto.DocumentSeries.Id)
            .FirstOrDefaultAsync();

        docSeries.CurrentNumber++;
        await _unitOfWork.DocumentSeries.UpdateAsync(docSeries);

        await _unitOfWork.CompleteAsync();
        return documentRequestDto;
    }

    public async Task<DocumentRequestDto> EditDocument(DocumentRequestDto documentRequestDto)
    {
        var userFirmId = await _unitOfWork.Users.GetUserFirmIdAsync(_userService.GetCurrentUserId());
        var document = await _unitOfWork.Documents.GetByIdAsync(documentRequestDto.Id);

        if (document == null)
        {
            throw new Exception("Document not found.");
        }

        document.IssueDate = documentRequestDto.IssueDate;
        document.DueDate = documentRequestDto.DueDate;
        document.DocumentTypeId = documentRequestDto.DocumentType?.Id;
        document.DocumentStatusId = documentRequestDto.DocumentStatus?.Id;
        document.ClientId = documentRequestDto.Client.Id;
        document.UserFirmId = userFirmId;

        await _unitOfWork.Documents.UpdateAsync(document);

        await UpdateDocumentProducts(document.Id, documentRequestDto.Products, userFirmId);

        await _unitOfWork.CompleteAsync();
        return documentRequestDto;
    }

    public async Task<DocumentRequestDto> GeneratePdfDocument(DocumentRequestDto documentRequestDto)
    {
        var activeUserFirm = await _unitOfWork.Users.GetUserFirmAsync(_userService.GetCurrentUserId());
        documentRequestDto.Seller = _mapper.Map<FirmDto>(activeUserFirm.Firm);

        //include invoice document class and generate pdf
        _pdfGenerationService.GenerateInvoicePdf(documentRequestDto);

        return documentRequestDto;
    }

    public async Task<DocumentStreamDto> GetInvoicePdfStream(DocumentRequestDto documentRequestDto)
    {
        var activeUserFirmId = await _unitOfWork.Users.GetUserFirmIdAsync(_userService.GetCurrentUserId());
        var activeUserFirm = await _unitOfWork.UserFirms.Query()
            .Where(uf => uf.UserFirmId == activeUserFirmId)
            .Include(uf => uf.Firm)
            .FirstOrDefaultAsync();

        var documentBankAccount = await _unitOfWork.Documents.Query()
            .Where(d => d.UserFirmId == activeUserFirmId)
            .Select(d => d.BankAccount)
            .FirstOrDefaultAsync();

        documentRequestDto.Seller = _mapper.Map<FirmDto>(activeUserFirm?.Firm);
        documentRequestDto.BankAccount = _mapper.Map<BankAccountDto>(documentBankAccount);

        var pdfContent = _pdfGenerationService.GetInvoicePdfStream(documentRequestDto);
        
        //include invoice document class and generate pdf
        return new DocumentStreamDto
        {
            DocumentNumber = documentRequestDto.DocumentNumber ??
                             documentRequestDto.DocumentSeries.CurrentNumber.ToString(),
            PdfContent = pdfContent
        };
    }

    public async Task<DocumentAutofillDto> GetDocumentAutofillInfo(Guid userId, int documentTypeId)
    {
        var userFirmId = await _unitOfWork.Users.Query()
            .Where(u => u.Id == userId)
            .Select(u => u.ActiveUserFirmId)
            .FirstOrDefaultAsync();

        if (userFirmId == null)
            return new DocumentAutofillDto();

        var dto = new DocumentAutofillDto
        {
            Clients = await _unitOfWork.Firms.Query()
                .Where(f => f.UserFirms.Any(uf => uf.UserId == userId && uf.IsClient))
                .ToListAsync(),
            DocumentSeries = await _unitOfWork.DocumentSeries.Query()
                .Where(ds => ds.UserFirmId == userFirmId && ds.DocumentTypeId == documentTypeId)
                .Include(ds => ds.DocumentType)
                .ToListAsync(),
            DocumentStatuses = await _unitOfWork.DocumentStatuses.Query().ToListAsync(),
            Products = await _unitOfWork.Products.Query()
                .Where(p => p.UserFirmId == userFirmId)
                .ToListAsync()
        };

        return dto;
    }

    public async Task<List<DocumentTableRecordDto>> GetDocumentTableRecords(int documentTypeId)
    {
        var activeUserFirmId = await _unitOfWork.Users.GetUserFirmIdAsync(_userService.GetCurrentUserId());
        if (activeUserFirmId == null) return new List<DocumentTableRecordDto>();

        var activeUserFirm = await _unitOfWork.UserFirms.Query()
            .Where(uf => uf.UserFirmId == activeUserFirmId)
            .Include(uf => uf.Firm)
            .FirstOrDefaultAsync();

        if (activeUserFirm == null) return new List<DocumentTableRecordDto>();

        var documents = await _unitOfWork.Documents.Query()
            .Where(document => document.UserFirmId == activeUserFirmId && document.DocumentTypeId == documentTypeId)
            .Include(document => document.Client)
            .Include(document => document.DocumentStatus)
            .ToListAsync();

        return _mapper.Map<List<DocumentTableRecordDto>>(documents);
    }

    public async Task<DocumentRequestDto> GetDocumentById(int documentId)
    {
        var document = await _unitOfWork.Documents.Query()
            .Where(d => d.Id == documentId)
            .Include(d => d.DocumentStatus)
            .Include(d => d.DocumentProducts)
            .ThenInclude(dp => dp.Product)
            .Include(d => d.Client)
            .FirstOrDefaultAsync();

        return _mapper.Map<DocumentRequestDto>(document);
    }

    public async Task DeleteDocuments(int[] documentIds)
    {
        var documents = await _unitOfWork.Documents.Query()
            .Include(dp => dp.DocumentProducts)
            .Where(d => documentIds.Contains(d.Id))
            .ToListAsync();

        await _unitOfWork.DocumentProducts.RemoveRangeAsync(documents.SelectMany(d => d.DocumentProducts));
        await _unitOfWork.Documents.RemoveRangeAsync(documents);

        await _unitOfWork.CompleteAsync();
    }

    public async Task<DashboardStatsDto> GetDashboardStats()
    {
        var activeUserFirmId = await _unitOfWork.Users.GetUserFirmIdAsync(_userService.GetCurrentUserId());
        if (activeUserFirmId == null) return new DashboardStatsDto();

        var activeUserFirm = await _unitOfWork.UserFirms.Query()
            .Where(uf => uf.UserFirmId == activeUserFirmId)
            .Include(uf => uf.User)
            .FirstOrDefaultAsync();

        var totalDocumentsTask = await _unitOfWork.Documents.GetTotalDocumentsAsync((int)activeUserFirmId);
        var totalClientsTask = await _unitOfWork.Firms.GetTotalClientsAsync(activeUserFirm!.User.Id);
        var totalProductsTask = await _unitOfWork.Products.GetTotalProductsAsync((int)activeUserFirmId);
        var totalBankAccountsTask = await _unitOfWork.BankAccounts.GetTotalBankAccountsAsync((int)activeUserFirmId);
        var monthlyTotalsTask = await GetMonthlyTotalsAsync((int)activeUserFirmId);

        return new DashboardStatsDto
        {
            TotalDocuments = totalDocumentsTask,
            TotalClients = totalClientsTask,
            TotalProducts = totalProductsTask,
            TotalBankAccounts = totalBankAccountsTask,
            MonthlyTotals = monthlyTotalsTask
        };
    }

    private async Task<List<MonthlyTotalDto>> GetMonthlyTotalsAsync(int firmId)
    {
        return await _unitOfWork.Documents.Query()
            .Where(d => d.UserFirmId == firmId && d.IssueDate.Year == DateTime.Now.Year && d.DocumentType!.Id == 1)
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

    public async Task TransformToStorno(int[] documentIds)
    {
        var activeUserFirmId = await _unitOfWork.Users.GetUserFirmIdAsync(_userService.GetCurrentUserId());
        if (activeUserFirmId == null)
            throw new Exception("User firm not found.");

        foreach (var documentId in documentIds)
        {
            var document = await _unitOfWork.Documents.Query()
                .Where(d => d.Id == documentId && d.UserFirmId == activeUserFirmId)
                .FirstOrDefaultAsync();

            if (document == null)
                throw new Exception("Document not found.");

            document.DocumentStatusId = 3;
            await _unitOfWork.Documents.UpdateAsync(document);
            await _unitOfWork.CompleteAsync();
        }
    }
}
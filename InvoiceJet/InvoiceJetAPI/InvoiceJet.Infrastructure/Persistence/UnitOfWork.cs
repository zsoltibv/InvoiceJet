using InvoiceJet.Domain.Interfaces;
using InvoiceJet.Domain.Interfaces.Repositories;
using InvoiceJet.Infrastructure.Persistence.Repositories;

namespace InvoiceJet.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly InvoiceJetDbContext _context;
    
    public UnitOfWork(InvoiceJetDbContext context)
    {
        _context = context;
        BankAccounts = new BankAccountRepository(context);
        DocumentProducts = new DocumentProductRepository(context);
        Documents = new DocumentRepository(context);
        DocumentSeries = new DocumentSeriesRepository(context);
        DocumentStatuses = new DocumentStatusRepository(context);
        DocumentTypes = new DocumentTypeRepository(context);
        Firms = new FirmRepository(context);
        Products = new ProductRepository(context);
        UserFirms = new UserFirmRepository(context);
        Users = new UserRepository(context);
    }

    public IBankAccountRepository BankAccounts { get; }
    public IDocumentProductRepository DocumentProducts { get; }
    public IDocumentRepository Documents { get; }
    public IDocumentSeriesRepository DocumentSeries { get; }
    public IDocumentStatusRepository DocumentStatuses { get; }
    public IDocumentTypeRepository DocumentTypes { get; }
    public IFirmRepository Firms { get; }
    public IProductRepository Products { get; }
    public IUserFirmRepository UserFirms { get; }
    public IUserRepository Users { get; }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
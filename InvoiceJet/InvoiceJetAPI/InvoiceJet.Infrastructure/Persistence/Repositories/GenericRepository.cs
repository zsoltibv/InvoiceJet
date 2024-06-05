using InvoiceJet.Domain.Interfaces.Repositories;
using InvoiceJet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InvoiceJet.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly InvoiceJetDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(InvoiceJetDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public IQueryable<T> Query()
    {
        return _dbSet.AsQueryable();
    }
}
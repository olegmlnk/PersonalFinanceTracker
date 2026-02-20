using FinanceTracker.Application.Contracts.Persistence;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _dbContext;

    public CategoryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Category>> GetAllAsync(TransactionType? type, CancellationToken ct)
    {
        var query = _dbContext.Categories.AsNoTracking();

        if (type.HasValue)
        {
            query = query.Where(x => x.Type == type.Value);
        }

        return query
            .OrderBy(x => x.Type)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);
    }

    public Task<Category?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct)
    {
        return _dbContext.Categories.AnyAsync(x => x.Id == id, ct);
    }

    public Task<bool> ExistsByNameAsync(string name, TransactionType type, CancellationToken ct)
    {
        return _dbContext.Categories.AnyAsync(
            x => x.Type == type && x.Name.ToLower() == name.ToLower(),
            ct);
    }

    public Task AddAsync(Category category, CancellationToken ct)
    {
        return _dbContext.Categories.AddAsync(category, ct).AsTask();
    }
}

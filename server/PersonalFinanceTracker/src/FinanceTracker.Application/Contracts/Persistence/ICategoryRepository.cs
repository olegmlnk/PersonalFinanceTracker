using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Contracts.Persistence;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync(TransactionType? type, CancellationToken ct);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);
    Task<bool> ExistsByNameAsync(string name, TransactionType type, CancellationToken ct);
    Task AddAsync(Category category, CancellationToken ct);
}

using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Contracts.Persistence;
using FinanceTracker.Application.DTOs.Summary;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _dbContext;

    public TransactionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return _dbContext.Transactions
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<List<Transaction>> GetAsync(TransactionQuery query, CancellationToken ct)
    {
        var data = _dbContext.Transactions
            .AsNoTracking()
            .Include(x => x.Category)
            .AsQueryable();

        if (query.From.HasValue)
        {
            data = data.Where(x => x.Date >= query.From.Value);
        }

        if (query.To.HasValue)
        {
            data = data.Where(x => x.Date <= query.To.Value);
        }

        if (query.Type.HasValue)
        {
            data = data.Where(x => x.Type == query.Type.Value);
        }

        if (query.CategoryId.HasValue)
        {
            data = data.Where(x => x.CategoryId == query.CategoryId.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim().ToLower();
            data = data.Where(x =>
                (x.Note != null && x.Note.ToLower().Contains(search)) ||
                x.Category.Name.ToLower().Contains(search));
        }

        data = ApplySorting(data, query);

        var skip = (query.Page - 1) * query.PageSize;

        return await data
            .Skip(skip)
            .Take(query.PageSize)
            .ToListAsync(ct);
    }

    public Task AddAsync(Transaction transaction, CancellationToken ct)
    {
        return _dbContext.Transactions.AddAsync(transaction, ct).AsTask();
    }

    public void Update(Transaction transaction)
    {
        _dbContext.Transactions.Update(transaction);
    }

    public void Remove(Transaction transaction)
    {
        _dbContext.Transactions.Remove(transaction);
    }

    public async Task<SummaryDto> GetSummaryAsync(DateOnly? from, DateOnly? to, CancellationToken ct)
    {
        var data = _dbContext.Transactions.AsNoTracking().AsQueryable();

        if (from.HasValue)
        {
            data = data.Where(x => x.Date >= from.Value);
        }

        if (to.HasValue)
        {
            data = data.Where(x => x.Date <= to.Value);
        }

        var income = await data
            .Where(x => x.Type == TransactionType.Income)
            .Select(x => x.Amount)
            .DefaultIfEmpty(0m)
            .SumAsync(ct);

        var expense = await data
            .Where(x => x.Type == TransactionType.Expense)
            .Select(x => x.Amount)
            .DefaultIfEmpty(0m)
            .SumAsync(ct);

        return new SummaryDto
        {
            Income = income,
            Expense = expense,
            Balance = income - expense
        };
    }

    private static IQueryable<Transaction> ApplySorting(IQueryable<Transaction> query, TransactionQuery request)
    {
        var sortBy = request.SortBy?.Trim().ToLowerInvariant() ?? "date";
        var isAsc = string.Equals(request.Order, "asc", StringComparison.OrdinalIgnoreCase);

        return (sortBy, isAsc) switch
        {
            ("amount", true) => query.OrderBy(x => x.Amount).ThenBy(x => x.Date),
            ("amount", false) => query.OrderByDescending(x => x.Amount).ThenByDescending(x => x.Date),
            ("category", true) => query.OrderBy(x => x.Category.Name).ThenBy(x => x.Date),
            ("category", false) => query.OrderByDescending(x => x.Category.Name).ThenByDescending(x => x.Date),
            (_, true) => query.OrderBy(x => x.Date).ThenBy(x => x.CreatedAt),
            _ => query.OrderByDescending(x => x.Date).ThenByDescending(x => x.CreatedAt)
        };
    }
}

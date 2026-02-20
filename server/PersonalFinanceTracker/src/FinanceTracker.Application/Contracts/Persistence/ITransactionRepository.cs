using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.DTOs.Summary;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Contracts.Persistence;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<Transaction>> GetAsync(TransactionQuery query, CancellationToken ct);
    Task AddAsync(Transaction transaction, CancellationToken ct);
    void Update(Transaction transaction);
    void Remove(Transaction transaction);
    Task<SummaryDto> GetSummaryAsync(DateOnly? from, DateOnly? to, CancellationToken ct);
}

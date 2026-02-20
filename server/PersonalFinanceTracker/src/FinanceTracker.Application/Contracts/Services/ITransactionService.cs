using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.DTOs.Transactions;

namespace FinanceTracker.Application.Contracts.Services;

public interface ITransactionService
{
    Task<List<TransactionDto>> GetTransactionsAsync(TransactionQuery query, CancellationToken ct);
    Task<TransactionDto> GetByIdAsync(Guid id, CancellationToken ct);
    Task<TransactionDto> CreateAsync(CreateTransactionRequest request, CancellationToken ct);
    Task<TransactionDto> UpdateAsync(Guid id, UpdateTransactionRequest request, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}

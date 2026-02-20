using FinanceTracker.Application.Contracts.Persistence;
using FinanceTracker.Application.Contracts.Services;
using FinanceTracker.Application.DTOs.Summary;

namespace FinanceTracker.Application.Services;

public class SummaryService : ISummaryService
{
    private readonly ITransactionRepository _transactionRepository;

    public SummaryService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public Task<SummaryDto> GetSummaryAsync(DateOnly? from, DateOnly? to, CancellationToken ct)
    {
        return _transactionRepository.GetSummaryAsync(from, to, ct);
    }
}

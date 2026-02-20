using FinanceTracker.Application.DTOs.Summary;

namespace FinanceTracker.Application.Contracts.Services;

public interface ISummaryService
{
    Task<SummaryDto> GetSummaryAsync(DateOnly? from, DateOnly? to, CancellationToken ct);
}

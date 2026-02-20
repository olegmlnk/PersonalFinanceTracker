using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Common.Models;

public class TransactionQuery
{
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
    public TransactionType? Type { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Search { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SortBy { get; set; } = "date";
    public string? Order { get; set; } = "desc";
}

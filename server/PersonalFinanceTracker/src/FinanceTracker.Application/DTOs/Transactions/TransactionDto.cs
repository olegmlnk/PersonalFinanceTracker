using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs.Transactions;

public class TransactionDto
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public TransactionType Type { get; set; }
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? Note { get; set; }
}

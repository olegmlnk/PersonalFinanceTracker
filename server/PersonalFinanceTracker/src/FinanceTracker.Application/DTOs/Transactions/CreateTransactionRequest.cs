using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs.Transactions;

public class CreateTransactionRequest
{
    public DateOnly Date { get; set; }
    public TransactionType Type { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string? Note { get; set; }
}

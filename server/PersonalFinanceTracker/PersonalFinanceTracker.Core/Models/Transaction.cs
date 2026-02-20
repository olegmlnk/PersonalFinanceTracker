using Microsoft.EntityFrameworkCore;
using PersonalFinanceTracker.Core.Enums;

namespace PersonalFinanceTracker.Core.Models;

public class Transaction : BaseEntity
{
    public DateOnly TransactionDate { get; set; }
    public TransactionType TransactionType { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    [Precision(18, 2)]
    public decimal Amount { get; set; }
    string? Note { get; set; }
}
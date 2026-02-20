using FinanceTracker.Domain.Common;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Category : BaseEntity
{
    public required string Name { get; set; }
    public TransactionType Type { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}

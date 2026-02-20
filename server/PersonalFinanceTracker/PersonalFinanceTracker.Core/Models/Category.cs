using PersonalFinanceTracker.Core.Enums;

namespace PersonalFinanceTracker.Core.Models;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TransactionType TransactionType { get; set; }
    
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
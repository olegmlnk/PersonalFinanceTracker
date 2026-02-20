using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs.Categories;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TransactionType Type { get; set; }
}

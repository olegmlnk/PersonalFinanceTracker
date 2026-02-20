using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.DTOs.Categories;

public class CreateCategoryRequest
{
    public string Name { get; set; } = string.Empty;
    public TransactionType Type { get; set; }
}

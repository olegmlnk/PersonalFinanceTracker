using FinanceTracker.Application.DTOs.Categories;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Contracts.Services;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetCategoriesAsync(TransactionType? type, CancellationToken ct);
    Task<CategoryDto> CreateAsync(CreateCategoryRequest request, CancellationToken ct);
}

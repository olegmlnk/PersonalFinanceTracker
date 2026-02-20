using AutoMapper;
using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Contracts.Persistence;
using FinanceTracker.Application.Contracts.Services;
using FinanceTracker.Application.DTOs.Categories;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> GetCategoriesAsync(TransactionType? type, CancellationToken ct)
    {
        var categories = await _categoryRepository.GetAllAsync(type, ct);
        return _mapper.Map<List<CategoryDto>>(categories);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request, CancellationToken ct)
    {
        var normalizedName = request.Name.Trim();

        if (string.IsNullOrWhiteSpace(normalizedName))
        {
            throw new BusinessRuleException("Category name is required.");
        }

        if (normalizedName.Length > 60)
        {
            throw new BusinessRuleException("Category name must not exceed 60 characters.");
        }

        var exists = await _categoryRepository.ExistsByNameAsync(normalizedName, request.Type, ct);
        if (exists)
        {
            throw new BusinessRuleException("A category with the same name and type already exists.");
        }

        var category = new Category
        {
            Name = normalizedName,
            Type = request.Type
        };

        await _categoryRepository.AddAsync(category, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return _mapper.Map<CategoryDto>(category);
    }
}

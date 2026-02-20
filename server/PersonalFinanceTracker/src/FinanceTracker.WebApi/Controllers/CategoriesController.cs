using FinanceTracker.Application.Contracts.Services;
using FinanceTracker.Application.DTOs.Categories;
using FinanceTracker.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CategoryDto>>> GetCategories(
        [FromQuery] TransactionType? type,
        CancellationToken ct)
    {
        var categories = await _categoryService.GetCategoriesAsync(type, ct);
        return Ok(categories);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CategoryDto>> CreateCategory(
        [FromBody] CreateCategoryRequest request,
        CancellationToken ct)
    {
        var category = await _categoryService.CreateAsync(request, ct);
        return Created($"/api/categories/{category.Id}", category);
    }
}

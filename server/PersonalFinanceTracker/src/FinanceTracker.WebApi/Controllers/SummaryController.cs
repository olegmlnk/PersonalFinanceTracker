using FinanceTracker.Application.Contracts.Services;
using FinanceTracker.Application.DTOs.Summary;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SummaryController : ControllerBase
{
    private readonly ISummaryService _summaryService;

    public SummaryController(ISummaryService summaryService)
    {
        _summaryService = summaryService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(SummaryDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<SummaryDto>> GetSummary(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        CancellationToken ct)
    {
        var summary = await _summaryService.GetSummaryAsync(from, to, ct);
        return Ok(summary);
    }
}

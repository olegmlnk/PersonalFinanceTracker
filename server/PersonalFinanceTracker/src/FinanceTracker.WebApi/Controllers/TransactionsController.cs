using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Contracts.Services;
using FinanceTracker.Application.DTOs.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<TransactionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TransactionDto>>> GetTransactions(
        [FromQuery] TransactionQuery query,
        CancellationToken ct)
    {
        var transactions = await _transactionService.GetTransactionsAsync(query, ct);
        return Ok(transactions);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionDto>> GetById(Guid id, CancellationToken ct)
    {
        var transaction = await _transactionService.GetByIdAsync(id, ct);
        return Ok(transaction);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TransactionDto>> Create(
        [FromBody] CreateTransactionRequest request,
        CancellationToken ct)
    {
        var transaction = await _transactionService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TransactionDto>> Update(
        Guid id,
        [FromBody] UpdateTransactionRequest request,
        CancellationToken ct)
    {
        var transaction = await _transactionService.UpdateAsync(id, request, ct);
        return Ok(transaction);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _transactionService.DeleteAsync(id, ct);
        return NoContent();
    }
}

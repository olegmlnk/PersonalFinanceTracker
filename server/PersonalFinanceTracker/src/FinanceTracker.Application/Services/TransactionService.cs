using AutoMapper;
using FluentValidation;
using FinanceTracker.Application.Common.Exceptions;
using FinanceTracker.Application.Common.Models;
using FinanceTracker.Application.Contracts.Persistence;
using FinanceTracker.Application.Contracts.Services;
using FinanceTracker.Application.DTOs.Transactions;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateTransactionRequest> _createValidator;
    private readonly IValidator<UpdateTransactionRequest> _updateValidator;
    private readonly IMapper _mapper;

    public TransactionService(
        ITransactionRepository transactionRepository,
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateTransactionRequest> createValidator,
        IValidator<UpdateTransactionRequest> updateValidator,
        IMapper mapper)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _mapper = mapper;
    }

    public async Task<List<TransactionDto>> GetTransactionsAsync(TransactionQuery query, CancellationToken ct)
    {
        query.Page = query.Page <= 0 ? 1 : query.Page;
        query.PageSize = query.PageSize <= 0 ? 20 : Math.Min(query.PageSize, 200);

        var transactions = await _transactionRepository.GetAsync(query, ct);
        return _mapper.Map<List<TransactionDto>>(transactions);
    }

    public async Task<TransactionDto> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id, ct);
        if (transaction is null)
        {
            throw new NotFoundException($"Transaction with id '{id}' was not found.");
        }

        return _mapper.Map<TransactionDto>(transaction);
    }

    public async Task<TransactionDto> CreateAsync(CreateTransactionRequest request, CancellationToken ct)
    {
        await _createValidator.ValidateAndThrowAsync(request, ct);

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, ct);
        if (category is null)
        {
            throw new NotFoundException($"Category with id '{request.CategoryId}' was not found.");
        }

        if (category.Type != request.Type)
        {
            throw new BusinessRuleException("Transaction type must match the selected category type.");
        }

        var transaction = new Transaction
        {
            Date = request.Date,
            Type = request.Type,
            CategoryId = request.CategoryId,
            Amount = request.Amount,
            Note = request.Note?.Trim()
        };

        await _transactionRepository.AddAsync(transaction, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        var created = await _transactionRepository.GetByIdAsync(transaction.Id, ct)
            ?? throw new NotFoundException($"Transaction with id '{transaction.Id}' was not found after create.");

        return _mapper.Map<TransactionDto>(created);
    }

    public async Task<TransactionDto> UpdateAsync(Guid id, UpdateTransactionRequest request, CancellationToken ct)
    {
        await _updateValidator.ValidateAndThrowAsync(request, ct);

        var transaction = await _transactionRepository.GetByIdAsync(id, ct);
        if (transaction is null)
        {
            throw new NotFoundException($"Transaction with id '{id}' was not found.");
        }

        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, ct);
        if (category is null)
        {
            throw new NotFoundException($"Category with id '{request.CategoryId}' was not found.");
        }

        if (category.Type != request.Type)
        {
            throw new BusinessRuleException("Transaction type must match the selected category type.");
        }

        transaction.Date = request.Date;
        transaction.Type = request.Type;
        transaction.CategoryId = request.CategoryId;
        transaction.Amount = request.Amount;
        transaction.Note = request.Note?.Trim();
        transaction.UpdatedAt = DateTime.UtcNow;

        _transactionRepository.Update(transaction);
        await _unitOfWork.SaveChangesAsync(ct);

        var updated = await _transactionRepository.GetByIdAsync(transaction.Id, ct)
            ?? throw new NotFoundException($"Transaction with id '{transaction.Id}' was not found after update.");

        return _mapper.Map<TransactionDto>(updated);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var transaction = await _transactionRepository.GetByIdAsync(id, ct);
        if (transaction is null)
        {
            throw new NotFoundException($"Transaction with id '{id}' was not found.");
        }

        _transactionRepository.Remove(transaction);
        await _unitOfWork.SaveChangesAsync(ct);
    }
}

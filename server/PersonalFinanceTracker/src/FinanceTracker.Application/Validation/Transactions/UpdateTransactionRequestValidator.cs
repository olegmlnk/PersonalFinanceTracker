using FluentValidation;
using FinanceTracker.Application.DTOs.Transactions;

namespace FinanceTracker.Application.Validation.Transactions;

public class UpdateTransactionRequestValidator : AbstractValidator<UpdateTransactionRequest>
{
    public UpdateTransactionRequestValidator()
    {
        RuleFor(x => x.Date)
            .NotEqual(default(DateOnly))
            .WithMessage("Date is required.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Transaction type is invalid.");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("CategoryId is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.Note)
            .MaximumLength(200)
            .WithMessage("Note must not exceed 200 characters.");
    }
}

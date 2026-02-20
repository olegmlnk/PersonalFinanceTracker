using FinanceTracker.Domain.Entities;
using FinanceTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext dbContext, CancellationToken ct = default)
    {
        if (await dbContext.Categories.AnyAsync(ct))
        {
            return;
        }

        var categories = new List<Category>
        {
            new() { Name = "Salary", Type = TransactionType.Income },
            new() { Name = "Freelance", Type = TransactionType.Income },
            new() { Name = "Gift", Type = TransactionType.Income },
            new() { Name = "Food", Type = TransactionType.Expense },
            new() { Name = "Transport", Type = TransactionType.Expense },
            new() { Name = "Rent", Type = TransactionType.Expense },
            new() { Name = "Entertainment", Type = TransactionType.Expense }
        };

        await dbContext.Categories.AddRangeAsync(categories, ct);
        await dbContext.SaveChangesAsync(ct);
    }
}

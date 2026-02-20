using AutoMapper;
using FluentValidation;
using FinanceTracker.Application.Contracts.Services;
using FinanceTracker.Application.DTOs.Transactions;
using FinanceTracker.Application.Mappings;
using FinanceTracker.Application.Services;
using FinanceTracker.Application.Validation.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceTracker.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var mapperConfiguration = new MapperConfiguration(config =>
        {
            config.AddProfile<MappingProfile>();
        });

        services.AddSingleton<IMapper>(_ => mapperConfiguration.CreateMapper());

        services.AddScoped<IValidator<CreateTransactionRequest>, CreateTransactionRequestValidator>();
        services.AddScoped<IValidator<UpdateTransactionRequest>, UpdateTransactionRequestValidator>();

        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ISummaryService, SummaryService>();

        return services;
    }
}

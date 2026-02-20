using AutoMapper;
using FinanceTracker.Application.DTOs.Categories;
using FinanceTracker.Application.DTOs.Transactions;
using FinanceTracker.Domain.Entities;

namespace FinanceTracker.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDto>();

        CreateMap<Transaction, TransactionDto>()
            .ForMember(
                destination => destination.CategoryName,
                options => options.MapFrom(source => source.Category.Name)
            );
    }
}

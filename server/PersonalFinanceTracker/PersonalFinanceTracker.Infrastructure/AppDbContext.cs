using Microsoft.EntityFrameworkCore;

namespace PersonalFinanceTracker.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    { }
    
}
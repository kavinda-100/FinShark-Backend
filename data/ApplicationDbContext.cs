using FinSharkMarket.models;
using Microsoft.EntityFrameworkCore;

namespace FinSharkMarket.data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
        
    }
    
    public DbSet<Stocks> Stocks { get; set; }
    public DbSet<Comments> Comments { get; set; }
}
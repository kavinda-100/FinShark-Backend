using FinSharkMarket.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinSharkMarket.data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
        
    }
    
    public DbSet<Stocks> Stocks { get; set; }
    public DbSet<Comments> Comments { get; set; }
    public DbSet<PortFolio> PortFolios { get; set; }
    
    // add the roles
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // create a many-to-many relationship
        builder.Entity<PortFolio>(x => x.HasKey(p => new {p.AppUserId, p.StockId}));
        // connect to them to the tables
        builder.Entity<PortFolio>()
            .HasOne(u => u.AppUser)
            .WithMany(s => s.PortFolios)
            .HasForeignKey(u => u.AppUserId);
        
        builder.Entity<PortFolio>()
            .HasOne(u => u.Stock)
            .WithMany(s => s.PortFolios)
            .HasForeignKey(u => u.StockId);
        
        // create the roles
        List<IdentityRole> roles = new List<IdentityRole>()
        {
            new IdentityRole {Name = "Admin", NormalizedName = "ADMIN"},
            new IdentityRole {Name = "User", NormalizedName = "USER"}
        };
        // seed the roles
        builder.Entity<IdentityRole>().HasData(roles);
    }
}
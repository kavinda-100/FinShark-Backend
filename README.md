# FinShark Stock Market Backend

This is the backend for the FinShark Stock Market project. It is a RESTful API that provides stock market data and user authentication.

### Installed Packages

```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

### Database Connection

```bash
"ConnectionStrings": {
    "DatabaseUrl": "Host=localhost; Database=FinSharkDb; Username=postgres; Password=your_password"
}
```

### since PostgresSQL Supports UTC time zone, we need to add the following code by creating a folder named **Utils** and adding a new class named **DateTimeUtils.cs**

```csharp
namespace FinSharkMarket.utils;

public static class DateTimeUtils
{
    public static DateTime ToUtc(DateTime dateTime)
    {
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
}
```

### Models
Stock.cs
```csharp
using System.ComponentModel.DataAnnotations.Schema;

namespace FinSharkMarket.models;

public class Stocks
{
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }
    [Column(TypeName = "varchar(255)")]
    public String Symbol { get; set; } = String.Empty;
    [Column(TypeName = "varchar(255)")]
    public String CompanyName { get; set; } = String.Empty;
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal LastDiv { get; set; }
    [Column(TypeName = "varchar(255)")]
    public String Industry { get; set; } = String.Empty;
    public long MarketCap { get; set; }
    // relationships
    public List<Comments> Comments { get; set; } = new List<Comments>();
    // date fields
    public DateTime CreatedAt { get; set; } = DateTimeUtils.ToUtc(DateTime.Now);
    public DateTime UpdatedAt { get; set; } = DateTimeUtils.ToUtc(DateTime.Now);
}
```

Comments.cs
```csharp
using System.ComponentModel.DataAnnotations.Schema;

namespace FinSharkMarket.models;

public class Comments
{
    [Column(TypeName = "uuid")]
    public Guid Id { get; set; }
    [Column(TypeName = "varchar(255)")]
    public String Title { get; set; } = String.Empty;
    [Column(TypeName = "varchar(255)")]
    public String Content { get; set; } = String.Empty;
    // foreign key (stock id)
    [Column(TypeName = "uuid")]
    public Guid StockId { get; set; }
    // navigation property
    public Stocks? Stock { get; set; }
    // date fields
    public DateTime CreatedAt { get; set; } = DateTimeUtils.ToUtc(DateTime.Now);
    public DateTime UpdatedAt { get; set; } = DateTimeUtils.ToUtc(DateTime.Now);
}
```

### create a new folder named **Data** and add a new class named **ApplicationDbContext.cs**

```csharp
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
```

### add the following to the Program.cs file

```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseUrl"));
});
```

### The following code is equivalent to the previous one

The main difference between the two lines is that the second one explicitly adds the Entity Framework Npgsql provider to the service collection,while the first one does not.However, in most cases, adding the provider explicitly is not necessary if you are already using UseNpgsql.
```csharp
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseUrl"));
});
```

### Database Migrations

```bash
dotnet ef migrations add comment // no colons ("comment" 🚨)
dotnet ef database update
```

### if dotnet ef is not found - install the following package

```bash
dotnet tool install --global dotnet-ef
```

### Running the API

```bash
dotnet run
```
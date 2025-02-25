# FinShark Stock Market Backend

This is the backend for the FinShark Stock Market project. It is a RESTful API that provides stock market data and user authentication.

### Installed Packages

```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
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

### Database Connection

```bash
"ConnectionStrings": {
    "DatabaseUrl": "Host=localhost; Database=FinSharkDb; Username=postgres; Password=your_password"
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

### Running the API

```bash
dotnet run
```
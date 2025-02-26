using FinSharkMarket.data;
using FinSharkMarket.interfaces.comments;
using FinSharkMarket.interfaces.stocks;
using FinSharkMarket.Repository.comments;
using FinSharkMarket.Repository.stocks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add db context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseUrl"));
});

//* Same thing as above,
// The main difference between the two lines is that
// the second one explicitly adds the
// Entity Framework Npgsql provider to the service collection,
// while the first one does not.However, in most cases, adding the
// provider explicitly is not necessary if you are already using UseNpgsql.

//? builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options =>
//? {
//?     options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseUrl"));
//? });

// Add repository's
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
using FinSharkMarket.data;
using FinSharkMarket.interfaces.comments;
using FinSharkMarket.interfaces.services;
using FinSharkMarket.interfaces.stocks;
using FinSharkMarket.models;
using FinSharkMarket.Repository.comments;
using FinSharkMarket.Repository.stocks;
using FinSharkMarket.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// prevent object cycle
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

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

// Identity Configuration
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Password settings for the user (in this case, keeping it simple)
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
}).AddEntityFrameworkStores<ApplicationDbContext>();
// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = 
        options.DefaultChallengeScheme =
            options.DefaultForbidScheme =
                options.DefaultScheme = 
                    options.DefaultSignInScheme = 
                        options.DefaultSignOutScheme = 
                            JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    // token validation parameters
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey =
            new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SignInKey"]!))
    };
});

// Add repository's
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
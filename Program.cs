using FinSharkMarket.data;
using FinSharkMarket.interfaces.comments;
using FinSharkMarket.interfaces.stocks;
using FinSharkMarket.Repository.comments;
using FinSharkMarket.Repository.stocks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var authority = builder.Configuration["Authentication:Schemes:Bearer:Authority"];
    options.AddSecurityDefinition(
        "oidc",
        new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OpenIdConnect,
            OpenIdConnectUrl = new Uri(authority + "/.well-known/openid-configuration")
        }
    );

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                        { Type = ReferenceType.SecurityScheme, Id = "oidc" },
                },
                new string[] { }
            }
        }
    );
});

// Kinde Auth setup
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        // These two lines map the Kinde user ID to Identity.Name (optional).
        options.MapInboundClaims = false;
        options.TokenValidationParameters.NameClaimType = "sub";
    });

builder.Services
    .AddAuthorization(options =>
    {
        options.AddPolicy("ReadPermission",
            policy => policy.RequireAssertion(
                context => context.User.Claims.Any(c => c.Type == "permissions" && c.Value == "read:data")
            ));
        options.AddPolicy("AdminRole",
            policy => policy.RequireAssertion(
                context => context.User.Claims.Any(c => c.Type == "roles" && c.Value == "admin")
            ));
        options.AddPolicy("UserRole",
            policy => policy.RequireAssertion(
                context => context.User.Claims.Any(c => c.Type == "roles" && c.Value == "user")
            ));
    });

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

// Add repository's
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.OAuthClientId("19127ac042564915bb73aca426869294");
        c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>
            { { "audience", builder.Configuration["Authentication:Schemes:Bearer:ValidAudiences:0"] ?? "" } });
        c.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
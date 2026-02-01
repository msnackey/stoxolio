using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stoxolio.Service.Data;
using Stoxolio.Service.Extensions;
using Stoxolio.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<StoxolioDbContext>(options =>
    options.UseSqlite(connectionString)
);

// Add Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]!;
var key = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true
    };
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173", "http://localhost:3000", "http://127.0.0.1:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Add Services
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Initialize Database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StoxolioDbContext>();
    db.Database.EnsureCreated();
    
    // Seed test data if development
    if (app.Environment.IsDevelopment())
    {
        SeedTestData(db);
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

// Map Endpoints
app.MapEndpoints();

app.Run();

// Seed test data
static void SeedTestData(StoxolioDbContext db)
{
    if (db.Users.Any()) return; // Don't seed if users already exist

    // Create test user
    var testUserPassword = "testpassword123";
    var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(testUserPassword, 13);
    
    var testUser = new Stoxolio.Service.Models.User
    {
        Username = "testuser",
        Email = "test@example.com",
        PasswordHash = passwordHash,
        CreatedAt = DateTime.UtcNow
    };

    db.Users.Add(testUser);
    db.SaveChanges();

    var categories = new[]
    {
        new Stoxolio.Service.Models.Category { Name = "Tech", Target = 10000m },
        new Stoxolio.Service.Models.Category { Name = "Finance", Target = 5000m },
        new Stoxolio.Service.Models.Category { Name = "Healthcare", Target = 3000m }
    };

    db.Categories.AddRange(categories);
    db.SaveChanges();

    var stocks = new[]
    {
        new Stoxolio.Service.Models.Stock
        {
            Name = "Apple Inc.",
            Ticker = "AAPL",
            Exchange = "NASDAQ",
            Sri = true,
            Shares = 100,
            Price = 150.50m,
            PrevPrice = 148.00m,
            Invest = true,
            CategoryId = 1
        },
        new Stoxolio.Service.Models.Stock
        {
            Name = "Microsoft Corporation",
            Ticker = "MSFT",
            Exchange = "NASDAQ",
            Sri = true,
            Shares = 50,
            Price = 320.00m,
            PrevPrice = 315.00m,
            Invest = true,
            CategoryId = 1
        },
        new Stoxolio.Service.Models.Stock
        {
            Name = "JPMorgan Chase",
            Ticker = "JPM",
            Exchange = "NYSE",
            Sri = false,
            Shares = 75,
            Price = 180.25m,
            PrevPrice = 178.50m,
            Invest = true,
            CategoryId = 2
        }
    };

    db.Stocks.AddRange(stocks);
    db.SaveChanges();
}

using Microsoft.EntityFrameworkCore;
using Stoxolio.Service.Data;
using Stoxolio.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173", "http://localhost:3000",
                "http://127.0.0.1:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Add Dependencies
builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

// Initialize Database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StoxolioDbContext>();
    db.Database.EnsureCreated();
    // EnsureCreated won't add new tables to an existing DB — do it manually
    db.Database.ExecuteSqlRaw(@"
        CREATE TABLE IF NOT EXISTS RefreshTokens (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            UserId INTEGER NOT NULL REFERENCES Users(Id) ON DELETE CASCADE,
            Token TEXT NOT NULL UNIQUE,
            Expires TEXT NOT NULL,
            IsRevoked INTEGER NOT NULL DEFAULT 0,
            CreatedAt TEXT NOT NULL
        )
    ");

    // Seed test data if development
    if (app.Environment.IsDevelopment())
    {
        TestData.SeedTestData(db);
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
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

// Map Endpoints
app.MapEndpoints();

app.Run();
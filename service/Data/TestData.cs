namespace Stoxolio.Service.Data;

public static class TestData
{
    public static void SeedTestData(StoxolioDbContext db)
    {
        if (db.Users.Any()) return; // Don't seed if users already exist

        // Create test user
        var testUserPassword = "testpassword123";
        var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(testUserPassword, 13);
        
        var testUser = new Models.User
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
            new Models.Category { Name = "Tech", Target = 0.50 },
            new Models.Category { Name = "Finance", Target = 0.25 },
            new Models.Category { Name = "Healthcare", Target = 0.25 }
        };

        db.Categories.AddRange(categories);
        db.SaveChanges();

        var stocks = new[]
        {
            new Models.Stock
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
            new Models.Stock
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
            new Models.Stock
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

        var transactions = new[]
        {
            new Models.Transaction
            {
                OrderId = "ORD-001",
                Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-10)),
                Time = TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(-10)),
                Product = "Apple Inc.",
                Isin = "US0378331005",
                Exchange = "NASDAQ",
                Shares = 100,
                Price = 150.50m,
                Value = 15050.00m,
                Fees = 9.99m,
                Total = 15059.99m
            },
            new Models.Transaction
            {
                OrderId = "ORD-002",
                Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5)),
                Time = TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(-5)),
                Product = "Microsoft Corporation",
                Isin = "US5949181045",
                Exchange = "NASDAQ",
                Shares = 50,
                Price = 320.00m,
                Value = 16000.00m,
                Fees = 9.99m,
                Total = 16009.99m
            },
            new Models.Transaction
            {
                OrderId = "ORD-003",
                Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)),
                Time = TimeOnly.FromDateTime(DateTime.UtcNow.AddDays(-2)),
                Product = "JPMorgan Chase",
                Isin = "US46625H1005",
                Exchange = "NYSE",
                Shares = 75,
                Price = 180.25m,
                Value = 13518.75m,
                Fees = 9.99m,
                Total = 13528.74m
            }
        };

        db.Transactions.AddRange(transactions);
        db.SaveChanges();
    }
}
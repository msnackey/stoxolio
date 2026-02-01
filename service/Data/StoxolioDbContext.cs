using Microsoft.EntityFrameworkCore;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Data;

public class StoxolioDbContext : DbContext
{
    public StoxolioDbContext(DbContextOptions<StoxolioDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Category configuration
        modelBuilder.Entity<Category>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Category>()
            .Property(c => c.Target)
            .HasPrecision(18, 2);

        // Stock configuration
        modelBuilder.Entity<Stock>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<Stock>()
            .HasOne(s => s.Category)
            .WithMany(c => c.Stocks)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Stock>()
            .Property(s => s.Price)
            .HasPrecision(18, 3);

        modelBuilder.Entity<Stock>()
            .Property(s => s.PrevPrice)
            .HasPrecision(18, 3);

        // Transaction configuration
        modelBuilder.Entity<Transaction>()
            .HasKey(t => t.OrderId);

        modelBuilder.Entity<Transaction>()
            .HasIndex(t => t.Isin);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Price)
            .HasPrecision(18, 3);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Value)
            .HasPrecision(18, 3);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Fees)
            .HasPrecision(18, 3);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Total)
            .HasPrecision(18, 3);
    }
}

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

        // Stock configuration
        modelBuilder.Entity<Stock>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<Stock>()
            .HasOne(s => s.Category)
            .WithMany(c => c.Stocks)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Decimal precision
        modelBuilder.Entity<Category>()
            .Property(c => c.Target)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Stock>()
            .Property(s => s.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Stock>()
            .Property(s => s.PrevPrice)
            .HasPrecision(18, 2);
    }
}

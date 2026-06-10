using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Data.Configurations;

public class StockConfiguration : BaseEntityConfiguration<Stock>
{
    public override void Configure(EntityTypeBuilder<Stock> builder)
    {
        base.Configure(builder);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Ticker)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(s => s.Exchange)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(s => s.Sri)
            .IsRequired();

        builder.Property(s => s.Shares)
            .IsRequired();

        builder.Property(s => s.Price)
            .IsRequired()
            .HasPrecision(18, 3);

        builder.Property(s => s.Invest)
            .IsRequired();

        builder.Property(s => s.CategoryId)
            .IsRequired();

        builder.HasOne(s => s.Category)
            .WithMany(c => c.Stocks)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(s => s.PrevPrice)
            .IsRequired()
            .HasPrecision(18, 3);
    }
}
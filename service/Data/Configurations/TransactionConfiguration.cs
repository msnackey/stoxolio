using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Data.Configurations;

public class TransactionConfiguration : BaseEntityConfiguration<Transaction>
{
    public override void Configure(EntityTypeBuilder<Transaction> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.Date)
            .IsRequired();

        builder.Property(t => t.Time)
            .IsRequired();

        builder.Property(t => t.Product)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Isin)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(t => t.Exchange)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.Shares)
            .IsRequired();

        builder.Property(t => t.Price)
            .IsRequired()
            .HasPrecision(18, 3);

        builder.Property(t => t.Value)
            .IsRequired()
            .HasPrecision(18, 3);

        builder.Property(t => t.Fees)
            .IsRequired()
            .HasPrecision(18, 3);

        builder.Property(t => t.Total)
            .IsRequired()
            .HasPrecision(18, 3);

        builder.Property(t => t.OrderId)
            .IsRequired()
            .HasMaxLength(50);
    }
}
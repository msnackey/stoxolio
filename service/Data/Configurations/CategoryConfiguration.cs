using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stoxolio.Service.Models;

namespace Stoxolio.Service.Data.Configurations;

public class CategoryConfiguration : BaseEntityConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Target)
            .HasPrecision(18, 2);
    }
}
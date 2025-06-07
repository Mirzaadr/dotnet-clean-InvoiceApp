using InvoiceApp.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceApp.Infrastructure.Persistence.Configurations;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("product");

        builder.HasKey(c => c.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedNever()
            .HasColumnName("id")
            .HasConversion(
                id => id.Value,
                value => new ProductId(value)
            );

        builder.Property(c => c.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(100);
        builder.Property(c => c.Description)
            .HasColumnName("description")
            .HasMaxLength(500);
        builder.Property(c => c.UnitPrice)
            .IsRequired()
            .HasColumnName("unit_price")
            .HasColumnType("decimal(18,2)");
        // builder.Property(c => c.StockQuantity)
        //     .IsRequired()
        //     .HasColumnName("stock_quantity")
        //     .HasColumnType("int");
        builder.Property(c => c.CreatedDate)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_date");
        // .HasConversion(
        //     v => v.HasValue ? v.Value.ToUniversalTime() : v, // Convert to UTC when saving
        //     v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v // Mark as UTC when reading
        // );
        builder.Property(c => c.UpdatedDate)
            .HasColumnName("updated_date");
            // .HasConversion(
            //     v => v.HasValue ? v.Value.ToUniversalTime() : v, // Convert to UTC when saving
            //     v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v // Mark as UTC when reading
            // );
    }
}
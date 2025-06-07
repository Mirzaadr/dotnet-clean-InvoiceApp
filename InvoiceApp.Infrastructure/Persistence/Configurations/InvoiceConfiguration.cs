using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Invoices;
using InvoiceApp.Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceApp.Infrastructure.Persistence.Configurations;

internal class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        ConfigureInvoiceTable(builder);
        ConfigureInvoiceItemsTable(builder);
    }

    public void ConfigureInvoiceItemsTable(EntityTypeBuilder<Invoice> builder)
    {
        builder.OwnsMany(i => i.Items, ib =>
        {
            ib.ToTable("invoice_item");

            ib.WithOwner().HasForeignKey("invoice_id");

            ib.HasKey(i => i.Id);
            ib.Property(i => i.Id)
                .ValueGeneratedNever()
                .HasColumnName("id")
                .HasConversion(
                    id => id.Value,
                    value => new InvoiceItemId(value)
                );
            ib.Property(i => i.ProductId)
                .IsRequired()
                .HasColumnName("product_id")
                .HasConversion(
                    id => id.Value,
                    value => new ProductId(value)
                );
            
            ib.Property(i => i.ProductName)
                .IsRequired()
                .HasColumnName("product_name")
                .HasMaxLength(100);

            ib.Property(i => i.Quantity)
                .IsRequired()
                .HasDefaultValue(1)
                .HasColumnName("quantity");

            ib.Property(i => i.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasColumnName("unit_price");
        });
        
        builder.Metadata.FindNavigation(nameof(Invoice.Items))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    public void ConfigureInvoiceTable(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("invoice");

        builder.HasKey(i => i.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedNever()
            .HasColumnName("id")
            .HasConversion(
                id => id.Value,
                value => new InvoiceId(value)
            );

        builder.Property(i => i.InvoiceNumber)
            .IsRequired()
            .HasColumnName("invoice_number")
            .HasMaxLength(50);
        

        builder.Property(i => i.ClientId)
            .IsRequired()
            .HasColumnName("client_id")
            .HasConversion(
                id => id.Value,
                value => new ClientId(value)
            );
        builder.Property(i => i.ClientName)
            .IsRequired()
            .HasColumnName("client_name")
            .HasMaxLength(100);

        builder.Property(i => i.IssueDate)
            .IsRequired()
            .HasColumnName("issue_date");
            // .HasConversion(
            //     v => v.ToUniversalTime(), // Convert to UTC when saving
            //     v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Mark as UTC when reading
            // );
        builder.Property(i => i.DueDate)
            .IsRequired()
            .HasColumnName("due_date");
            // .HasConversion(
            //     v => v.ToUniversalTime(), // Convert to UTC when saving
            //     v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Mark as UTC when reading
            // );
        
        builder.Property(i => i.Status)
            .IsRequired()
            .HasColumnName("status")
            .HasConversion(
                v => v.Value,
                v => InvoiceStatus.From(v)
            );

        builder.Property(i => i.CreatedDate)
            .HasDefaultValueSql("now()")
            .HasColumnName("created_date");
            // .HasConversion(
            //     v => v.HasValue ? v.Value.ToUniversalTime() : v, // Convert to UTC when saving
            //     v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v // Mark as UTC when reading
            // );
        builder.Property(i => i.UpdatedDate)
            .HasColumnName("updated_date");
            // .HasConversion(
            //     v => v.HasValue ? v.Value.ToUniversalTime() : v, // Convert to UTC when saving
            //     v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v // Mark as UTC when reading
            // );
    }
}
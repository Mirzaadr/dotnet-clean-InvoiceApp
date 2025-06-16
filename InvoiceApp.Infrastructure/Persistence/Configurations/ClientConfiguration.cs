using InvoiceApp.Domain.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceApp.Infrastructure.Persistence.Configurations;

internal class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("client");

        builder.HasKey(c => c.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedNever()
            .HasColumnName("id")
            .HasConversion(
                id => id.Value,
                value => ClientId.FromGuid(value)
            );

        builder.Property(c => c.Name)
            .IsRequired()
            .HasColumnName("name")
            .HasMaxLength(100);
        builder.Property(c => c.Email)
            .HasColumnName("email");
        builder.Property(c => c.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(15);
        builder.Property(c => c.Address)
            .HasColumnName("address")
            .HasMaxLength(200);
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
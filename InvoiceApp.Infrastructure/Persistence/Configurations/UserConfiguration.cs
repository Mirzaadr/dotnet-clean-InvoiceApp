using InvoiceApp.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceApp.Infrastructure.Persistence.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");

        builder.HasKey(c => c.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedNever()
            .HasColumnName("id")
            .HasConversion(
                id => id.Value,
                value => UserId.FromGuid(value)
            );

        builder.Property(c => c.Username)
            .IsRequired()
            .HasColumnName("username")
            .HasMaxLength(100);
        builder.Property(c => c.PasswordHash)
            .HasColumnName("password_hash");
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
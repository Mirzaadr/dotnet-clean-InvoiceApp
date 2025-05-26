namespace InvoiceApp.Domain.Commons.Models;

public class BaseEntity<TId> : IEquatable<BaseEntity<TId>>
    where TId : notnull
{
    public TId Id { get; private set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    protected BaseEntity(TId id, DateTime? createdDate, DateTime? updatedDate)
    {
      Id = id;
      CreatedDate = createdDate ?? DateTime.UtcNow;
      UpdatedDate = updatedDate ?? DateTime.UtcNow;
    }

    public override bool Equals(object? other)
    {
      return other is BaseEntity<TId> entity && Id.Equals(entity.Id);
    }

    public static bool operator ==(BaseEntity<TId> left, BaseEntity<TId> right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(BaseEntity<TId> left, BaseEntity<TId> right)
    {
      return !Equals(left, right);
    }

    public override int GetHashCode()
    {
      return Id.GetHashCode();
    }

    public bool Equals(BaseEntity<TId>? other)
    {
      return Equals((object?)other);
    }

    #pragma warning disable CS8618
    protected BaseEntity()
    {}
    #pragma warning disable CS8618
}
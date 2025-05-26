namespace InvoiceApp.Domain.Commons.Models;

public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public static bool operator ==(ValueObject left, ValueObject right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(ValueObject left, ValueObject right)
    {
      return !Equals(left, right);
    }

    public override int GetHashCode() =>
        GetEqualityComponents().Aggregate(1, (current, obj) =>
            HashCode.Combine(current, obj?.GetHashCode() ?? 0));

    public bool Equals(ValueObject? other)
    {
        return Equals((object?)other);
    }
}
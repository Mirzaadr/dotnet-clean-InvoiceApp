using InvoiceApp.Domain.Commons.Models;

namespace InvoiceApp.Domain.Users;

public sealed class UserId : ValueObject
{
    public Guid Value { get; }

    private UserId(Guid value) => Value = value;

    public static UserId New() => new(Guid.NewGuid());
    public static UserId FromGuid(Guid guid) => new(guid);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
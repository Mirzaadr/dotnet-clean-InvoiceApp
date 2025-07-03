using InvoiceApp.Domain.Commons.Models;

namespace InvoiceApp.Domain.Clients;

public sealed class ClientId : ValueObject
{
    public Guid Value { get; }

    private ClientId(Guid value) => Value = value;

    public static ClientId New() => new(Guid.NewGuid());
    public static ClientId FromGuid(Guid guid) => new(guid);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}

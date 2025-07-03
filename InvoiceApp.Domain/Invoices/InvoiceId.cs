using InvoiceApp.Domain.Commons.Models;

namespace InvoiceApp.Domain.Invoices;

public sealed class InvoiceId : ValueObject
{
    public Guid Value { get; }

    private InvoiceId(Guid value)
    {
        Value = value;
    }

    public static InvoiceId New() => new(Guid.NewGuid());

    public static InvoiceId FromGuid(Guid guid) => new(guid);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}

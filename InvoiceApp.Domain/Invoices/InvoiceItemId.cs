using InvoiceApp.Domain.Commons.Models;

namespace InvoiceApp.Domain.Invoices;

public class InvoiceItemId : ValueObject
{
    public Guid Value { get; }

    private InvoiceItemId(Guid value)
    {
        Value = value;
    }

    public static InvoiceItemId New() => new(Guid.NewGuid());

    public static InvoiceItemId FromGuid(Guid guid) => new(guid);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
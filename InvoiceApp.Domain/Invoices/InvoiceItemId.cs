using InvoiceApp.Domain.Commons.Models;

namespace InvoiceApp.Domain.Invoices;

public class InvoiceItemId : ValueObject
{
    public Guid Value { get; }

    public InvoiceItemId(Guid value)
    {
        Value = value;
    }

    public static InvoiceItemId New() => new(Guid.NewGuid());

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
using InvoiceApp.Domain.Commons.Models;

namespace InvoiceApp.Domain.Invoices;

public sealed class InvoiceStatus : ValueObject
{
    public static readonly InvoiceStatus Draft = new("Draft");
    public static readonly InvoiceStatus Sent = new("Sent");
    public static readonly InvoiceStatus Paid = new("Paid");

    public string Value { get; }

    private InvoiceStatus(string value) => Value = value;

    public static InvoiceStatus From(string value)
    {
        return value switch
        {
            "Draft" => Draft,
            "Sent" => Sent,
            "Paid" => Paid,
            _ => throw new ArgumentException($"Invalid status: {value}")
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}

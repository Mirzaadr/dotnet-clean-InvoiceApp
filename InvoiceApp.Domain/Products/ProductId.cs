using InvoiceApp.Domain.Commons.Models;

namespace InvoiceApp.Domain.Products;

public class ProductId : ValueObject
{
    public Guid Value { get; }

    public ProductId(Guid value) => Value = value;

    public static ProductId New() => new(Guid.NewGuid());

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
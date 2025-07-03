using InvoiceApp.Domain.Commons.Models;

namespace InvoiceApp.Domain.Products;

public class ProductId : ValueObject
{
    public Guid Value { get; }

    private ProductId(Guid value) => Value = value;

    public static ProductId New() => new(Guid.NewGuid());
    
    public static ProductId FromGuid(Guid guid) => new(guid);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
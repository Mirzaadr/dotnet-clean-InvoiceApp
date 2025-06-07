using InvoiceApp.Domain.Commons.Models;

namespace InvoiceApp.Domain.Products;

public class Product : BaseEntity<ProductId>
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public double UnitPrice { get; private set; }

    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Product() {}
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public Product(
      ProductId id,
      string name,
      double unitPrice,
      string? description,
      DateTime? createdDate,
      DateTime? updatedDate
    ) : base(id, createdDate, updatedDate)
    {
        Name = name;
        UnitPrice = unitPrice;
        Description = description;
    }

    public void UpdatePrice(double newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentException("Price cannot be negative.");

        UnitPrice = newPrice;
        UpdatedDate = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string? description)
    {
        Name = name;
        Description = description;
        UpdatedDate = DateTime.UtcNow;
    }
}
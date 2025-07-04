using InvoiceApp.Domain.Commons.Models;

namespace InvoiceApp.Domain.Products;

public class Product : BaseEntity<ProductId>
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public double UnitPrice { get; private set; }

    private Product(
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

    public static Product Create(
        string name,
        double unitPrice,
        string? description
    )
    {
        return new(
            ProductId.New(),
            name,
            unitPrice,
            description,
            DateTime.UtcNow,
            DateTime.UtcNow
        );
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
        // Name = name;
        Description = description;
        UpdatedDate = DateTime.UtcNow;
    }
}
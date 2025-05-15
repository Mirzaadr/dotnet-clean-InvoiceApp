using InvoiceApp.Domain.Common.Models;
using InvoiceApp.Domain.Invoices;

namespace InvoiceApp.Domain.Invoices;

public class LineItem : BaseEntity<LineItemId>
{
    // public InvoiceId InvoiceId { get; set; }
    // public virtual Invoice Invoice { get; set; } = null!;
    public string Name { get; set; }
    public string Description { get; set; }
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
    public double Amount => Quantity * UnitPrice;

    private LineItem(
      LineItemId id,
      string name,
      string description,
      int qty,
      double unitPrice,
      DateTime createdDate,
      DateTime updatedDate
    ) : base(id, createdDate, updatedDate)
    {
        Name = name;
        Description = description;
        Quantity = qty;
        UnitPrice = unitPrice;
    }

    public static LineItem Create(
      // Guid id,
      string name,
      string description,
      int qty,
      double unitPrice
    )
    {
        return new(
          new LineItemId(Guid.NewGuid()),
          name,
          description,
          qty,
          unitPrice,
          DateTime.UtcNow,
          DateTime.UtcNow
        );
    }
}
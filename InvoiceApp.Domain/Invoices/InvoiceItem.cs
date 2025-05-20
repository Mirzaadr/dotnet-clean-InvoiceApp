using InvoiceApp.Domain.Common.Models;
using InvoiceApp.Domain.Products;

namespace InvoiceApp.Domain.Invoices;

public class InvoiceItem : BaseEntity<InvoiceItemId>
{
    public ProductId ProductId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public double UnitPrice { get; private set; }
    public double TotalPrice => UnitPrice * Quantity;

    public InvoiceItem(
      InvoiceItemId id,
      ProductId productId,
      string productName,
      double unitPrice,
      int quantity,
      DateTime? createdTime,
      DateTime? updatedTime
    ) : base(id, createdTime, updatedTime)
    {
        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
    }
}
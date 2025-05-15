using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Common.Models;

namespace InvoiceApp.Domain.Invoices;

public class Invoice : BaseEntity<InvoiceId>
{
    public ClientId ClientId { get; private set; }
    public DateTime InvoiceDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public InvoiceStatus InvoiceStatus { get; private set; }
    public double TotalAmount { get; private set; }
    private readonly List<LineItem> _lineItems = new();
    public IReadOnlyList<LineItem> LineItems => _lineItems.AsReadOnly();

    private Invoice(
      InvoiceId id,
      ClientId clientId,
      DateTime invoiceDate,
      DateTime dueDate,
      InvoiceStatus status,
      double total,
      List<LineItem> listItems,
      DateTime createdDate,
      DateTime updatedDate
    ) : base(id, createdDate, updatedDate)
    {
        ClientId = clientId;
        InvoiceDate = invoiceDate;
        DueDate = dueDate;
        InvoiceStatus = status;
        _lineItems = listItems;
        TotalAmount = total;
    }

    public static Invoice Create(
      // Guid id,
      ClientId clientId,
      DateTime invoiceDate,
      DateTime dueDate,
      InvoiceStatus status,
      // double total,
      List<LineItem> listItems
    )
    {
        
        return new(
          new InvoiceId(Guid.NewGuid()),
          clientId,
          invoiceDate,
          dueDate,
          status,
          // total,
          0,
          listItems,
          DateTime.UtcNow,
          DateTime.UtcNow
        );
    }

    public void UpdateStatus(InvoiceStatusType newStatus)
    {
        InvoiceStatus = InvoiceStatus.ChangeStatus(newStatus); 
    }
}
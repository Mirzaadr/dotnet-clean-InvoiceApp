using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Commons.Models;
using InvoiceApp.Domain.Products;

namespace InvoiceApp.Domain.Invoices;

public class Invoice : BaseEntity<InvoiceId>
{
    public ClientId ClientId { get; private set; }
    public string ClientName { get; private set; }
    public string InvoiceNumber { get; private set; }
    public DateTime IssueDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public InvoiceStatus Status { get; private set; }
    private readonly List<InvoiceItem> _items = new();
    public IReadOnlyList<InvoiceItem> Items => _items.AsReadOnly();

    public double TotalAmount => _items.Sum(i => i.TotalPrice);

    private Invoice(
      InvoiceId id,
      ClientId clientId,
      string? clientName,
      string invoiceNum,
      DateTime issueDate,
      DateTime dueDate,
      InvoiceStatus status,
      List<InvoiceItem> listItems,
      DateTime createdDate,
      DateTime updatedDate
    ) : base(id, createdDate, updatedDate)
    {
        ClientId = clientId;
        ClientName = clientName ?? string.Empty;
        InvoiceNumber = invoiceNum;
        IssueDate = issueDate;
        DueDate = dueDate;
        Status = status;
        _items = listItems;
    }

    public static Invoice Create(
      ClientId clientId,
      string? clientName,
      string invoiceNum,
      DateTime issueDate,
      DateTime dueDate,
      InvoiceStatus status,
      List<InvoiceItem> items
    )
    {

        return new(
          InvoiceId.New(),
          clientId,
          clientName,
          invoiceNum,
          issueDate,
          dueDate,
          status,
          items,
          DateTime.UtcNow,
          DateTime.UtcNow
        );
    }

    public void AddItem(Product product, int quantity)
    {
        if (Status != InvoiceStatus.Draft)
            throw new InvalidOperationException("Cannot modify a finalized invoice.");

        var item = InvoiceItem.Create(
            product.Id,
            product.Name,
            product.UnitPrice,
            quantity
        );

        _items.Add(item);
    }

    public void UpdateItems(List<InvoiceItem> updatedItems)
    {
        if (Status != InvoiceStatus.Draft)
            throw new InvalidOperationException("Cannot modify a finalized invoice.");

        _items.Clear();
        _items.AddRange(updatedItems);
    }

    public double GetSubtotal() => _items.Sum(i => i.TotalPrice);

    public void MarkAsSent()
    {
        if (Status != InvoiceStatus.Draft)
            throw new InvalidOperationException("Only draft invoices can be sent.");
        Status = InvoiceStatus.Sent;
    }

    public void MarkAsPaid()
    {
        if (Status == InvoiceStatus.Paid)
            throw new InvalidOperationException("Invoice is already paid.");
        Status = InvoiceStatus.Paid;
    }

    public void UpdateClient(Guid clientId, string clientName)
    {
        ClientId = ClientId.FromGuid(clientId);
        ClientName = clientName;
    }

    public void UpdateInvoiceDates(DateTime issueDate, DateTime dueDate)
    {
        if (issueDate > dueDate)
            throw new InvalidOperationException("Issue date cannot be later than due date.");

        IssueDate = issueDate;
        DueDate = dueDate;
    }
}
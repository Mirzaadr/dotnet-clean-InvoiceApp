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
    // public double TotalAmount { get; private set; }
    private readonly List<InvoiceItem> _items = new();
    public IReadOnlyList<InvoiceItem> Items => _items.AsReadOnly();

    public double TotalAmount => _items.Sum(i => i.TotalPrice);

    #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Invoice() {}
    #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

    public Invoice(
      InvoiceId id,
      ClientId clientId,
      string? clientName,
      string invoiceNum,
      DateTime issueDate,
      DateTime dueDate,
      InvoiceStatus status,
    //   double total,
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
        // TotalAmount = total;
    }

    public static Invoice Create(
      // Guid id,
      ClientId clientId,
      string? clientName,
      string invoiceNum,
      DateTime issueDate,
      DateTime dueDate,
      InvoiceStatus status,
      // double total,
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

        var item = new InvoiceItem(
            InvoiceItemId.New(),
            product.Id,
            product.Name,
            product.UnitPrice,
            quantity,
            null,
            null
        );

        _items.Add(item);
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
        ClientId = new ClientId(clientId);
        ClientName = clientName;
    }

    public void UpdateItems(List<InvoiceItem> updatedItems)
    {
        _items.Clear();
        _items.AddRange(updatedItems);
    }
    
    public void UpdateInvoiceDates(DateTime issueDate, DateTime dueDate)
    {
        if (issueDate > dueDate)
            throw new InvalidOperationException("Issue date cannot be later than due date.");
        
        IssueDate = issueDate;
        DueDate = dueDate;
    }
}
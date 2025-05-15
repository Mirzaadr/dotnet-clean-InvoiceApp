using InvoiceApp.Domain.Common.Models;
using InvoiceApp.Domain.Invoices;

namespace InvoiceApp.Domain.Clients;

public class Client : BaseEntity<ClientId>
{
    private readonly List<Invoice> _invoices = new();
    public string Name { get; private set; } = string.Empty;
    public string? Address { get; private set; }
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public IReadOnlyList<Invoice> Invoices => _invoices.AsReadOnly();

    private Client(
      ClientId id,
      string name,
      string? address,
      string? email,
      string? phoneNumber,
      DateTime createdDate,
      DateTime updatedDate
    ) : base(id, createdDate, updatedDate)
    {
        Name = name;
        Address = address;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public static Client Create(
      // int id,
      string name,
      string? address,
      string? email,
      string? phoneNumber
    )
    {
        return new(
          new ClientId(Guid.NewGuid()),
          name,
          address,
          email,
          phoneNumber,
          DateTime.UtcNow,
          DateTime.UtcNow
        );
    }

    private Client()
    {}
}
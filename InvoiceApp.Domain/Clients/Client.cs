using InvoiceApp.Domain.Commons.Models;
using InvoiceApp.Domain.Invoices;

namespace InvoiceApp.Domain.Clients;

public class Client : BaseEntity<ClientId>
{
    public string Name { get; private set; } = string.Empty;
    public string? Address { get; private set; }
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }

    private Client(
      ClientId id,
      string name,
      string? address,
      string? email,
      string? phoneNumber,
      DateTime? createdDate,
      DateTime? updatedDate
    ) : base(id, createdDate, updatedDate)
    {
        Name = name;
        Address = address;
        Email = email;
        PhoneNumber = phoneNumber;
    }
    
    public static Client Create(
      string name,
      string? address,
      string? email,
      string? phoneNumber
    )
    {
      return new(
        ClientId.New(),
        name,
        address,
        email,
        phoneNumber,
        DateTime.UtcNow,
        DateTime.UtcNow
      );
    }
    
  public void Update(
    string name,
    string? address,
    string? email,
    string? phoneNumber,
    DateTime? updatedDate
  )
  {
    // Name = name;
    Address = address;
    Email = email;
    PhoneNumber = phoneNumber;
    UpdatedDate = updatedDate ?? DateTime.UtcNow;
  }

    private Client()
  { }
}
namespace InvoiceApp.Application.DTOs;

public class ClientDto
{
  public Guid Id { get; set; }
  public string Name { get; set; } = "";
  public string? Address { get; set; } = "";
  public string? Email { get; set; } = "";
  public string? PhoneNumber { get; set; } = "";
  public DateTime? CreatedDate { get; set; }
  public DateTime? UpdatedDate { get; set; }
}
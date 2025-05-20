namespace InvoiceApp.Application.DTOs;

public class InvoiceFormDTO
{
  public Guid Id { get; set; }
  public Guid ClientId { get; set; }
  public string InvoiceNumber { get; set; } = "";
  public DateTime IssueDate { get; set; }
  public DateTime DueDate { get; set; }
  public double TotalAmount { get; set; }
  public string Status { get; set; } = "Draft";
  public List<InvoiceItemDto> Items { get; set; } = new();
}

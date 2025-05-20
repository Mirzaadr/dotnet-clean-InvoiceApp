namespace InvoiceApp.Application.DTOs;

public class InvoiceDTO
{
  public Guid Id { get; set; }
  public string InvoiceNumber { get; set; } = "";
  public Guid ClientId { get; set; }
  public string ClientName { get; set; } = "";
  public DateTime IssueDate { get; set; }
  public DateTime DueDate { get; set; }
  public double TotalAmount { get; set; }
  public string Status { get; set; } = "Draft";
  public List<InvoiceItemDto> Items { get; set; } = new();
}


public class InvoiceItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
    public double LineTotal { get; set; }
}

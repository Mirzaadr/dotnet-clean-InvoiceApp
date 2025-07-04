using System.ComponentModel.DataAnnotations;

namespace InvoiceApp.Application.DTOs;

public class InvoiceDTO
{
  public Guid Id { get; set; }
  public string InvoiceNumber { get; set; } = "";
  public Guid ClientId { get; set; }
  [Required(ErrorMessage = "Client cannot be empty")]
  public string ClientName { get; set; } = "";
  public string? ClientEmail { get; set; }
  public string? ClientAddress { get; set; }
  public string? ClientPhone { get; set; }
  public DateTime IssueDate { get; set; }
  public DateTime DueDate { get; set; }
  public double TotalAmount { get; set; }
  public string Status { get; set; } = "Draft";
  public List<InvoiceItemDto> Items { get; set; } = new();
  
  public DateTime? CreatedDate { get; set; }
  public DateTime? UpdatedDate { get; set; }
}


public class InvoiceItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    [Required(ErrorMessage = "Product cannot be empty")]
    public string ProductName { get; set; } = "";
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
    public double LineTotal { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

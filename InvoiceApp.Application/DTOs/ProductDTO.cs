namespace InvoiceApp.Application.DTOs;

public class ProductDto
{
  public Guid Id { get; set; }
  public string Name { get; set; } = "";
  public string? Description { get; set; } = "";
  public double Price { get; set; }
  public DateTime? CreatedDate { get; set; }
  public DateTime? UpdatedDate { get; set; }
}

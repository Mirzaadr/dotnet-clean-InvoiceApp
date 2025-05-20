using InvoiceApp.Application.DTOs;
using InvoiceApp.Domain.Invoices;

namespace InvoiceApp.Application.Commons.Mappers;

public static class InvoiceMapper
{
  public static InvoiceDTO ToDto(Invoice invoice)
  {
    return new InvoiceDTO
    {
      Id = invoice.Id.Value,
      InvoiceNumber = invoice.InvoiceNumber,
      ClientId = invoice.ClientId.Value,
      ClientName = invoice.ClientName,
      IssueDate = invoice.IssueDate,
      DueDate = invoice.DueDate,
      Status = invoice.Status.ToString(),
      TotalAmount = invoice.TotalAmount,
      Items = invoice.Items.Select(item => new InvoiceItemDto
      {
        ProductId = item.ProductId.Value,
        ProductName = item.ProductName,
        Quantity = item.Quantity,
        UnitPrice = item.UnitPrice,
        LineTotal = item.TotalPrice
      }).ToList()
    };
  }
}
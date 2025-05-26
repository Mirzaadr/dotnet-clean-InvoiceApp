namespace InvoiceApp.Application.DTOs;


public class SummaryDto
{
  public int TotalInvoices { get; set; }
  public double TotalInvoicedAmount { get; set; }
  public int TotalClients { get; set; }
  public int TotalProducts { get; set; }

  // Product summary
  public string ProductMostExpensive { get; set; } = "";
  public double ProductMaxPrice { get; set; }

  public string ProductCheapest { get; set; } = "";
  public double ProductMinPrice { get; set; }

  // Invoices
  public List<InvoiceDTO> RecentInvoices { get; set; } = new();
}
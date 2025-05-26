namespace InvoiceApp.Web.Models.ViewModels;

public class PaginationViewModel
{
  public int CurrentPage { get; set; }
  public int TotalPages { get; set; }
  public int PageSize { get; set; }
  public int TotalItems { get; set; }
  public string? SearchTerm { get; set; }
}
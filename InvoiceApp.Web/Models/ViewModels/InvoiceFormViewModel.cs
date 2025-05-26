using InvoiceApp.Application.DTOs;

namespace InvoiceApp.Web.Models.ViewModels;

public class InvoiceFormViewModel
{
    public InvoiceDTO Invoice { get; set; } = null!;
    public List<ClientDto> Clients { get; set; } = null!;
    public List<ProductDto> Products { get; set; } = null!;
}
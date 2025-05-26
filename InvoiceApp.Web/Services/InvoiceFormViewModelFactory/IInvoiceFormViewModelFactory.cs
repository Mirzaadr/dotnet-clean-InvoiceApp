using InvoiceApp.Application.DTOs;
using InvoiceApp.Web.Models.ViewModels;

namespace InvoiceApp.Web.Services;

public interface IInvoiceFormViewModelFactory
{
    Task<InvoiceFormViewModel> CreateAsync(InvoiceDTO invoice, CancellationToken cancellationToken = default);
}
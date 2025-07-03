using InvoiceApp.Application.DTOs;

namespace InvoiceApp.Application.Commons.Interface;

public interface IPdfService
{
    byte[] GenerateInvoicePdf(InvoiceDTO invoice);
}
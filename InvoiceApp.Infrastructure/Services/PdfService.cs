namespace InvoiceApp.Infrastructure.Services;

// using DinkToPdf;
// using DinkToPdf.Contracts;
using InvoiceApp.Application.Commons.Interface;
using InvoiceApp.Application.DTOs;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Drawing;
using QuestPDF.Previewer;
using System.Globalization;
using System.Text;

public class PdfService : IPdfService
{
    public byte[] GenerateInvoicePdf(InvoiceDTO invoice)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial"));

                page.Header()
                    .Text($"Invoice #{invoice.InvoiceNumber}")
                    .FontSize(20)
                    .Bold();

                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    col.Item().Text($"Client: {invoice.ClientName}");
                    col.Item().Text($"Issue Date: {invoice.IssueDate:dd MMM yyyy}");
                    col.Item().Text($"Due Date: {invoice.DueDate:dd MMM yyyy}");

                    col.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);

                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3); // Product
                            columns.RelativeColumn(1); // Qty
                            columns.RelativeColumn(2); // Unit Price
                            columns.RelativeColumn(2); // Total
                        });

                        // Header row
                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("Product").Bold();
                            header.Cell().Element(CellStyle).Text("Qty").Bold();
                            header.Cell().Element(CellStyle).Text("Unit Price").Bold();
                            header.Cell().Element(CellStyle).Text("Total").Bold();

                            static IContainer CellStyle(IContainer container) =>
                                container.DefaultTextStyle(x => x.SemiBold()).Padding(5).Background(Colors.Grey.Lighten2);
                        });

                        // Item rows
                        foreach (var item in invoice.Items)
                        {
                            table.Cell().Element(CellStyle).Text(item.ProductName);
                            table.Cell().Element(CellStyle).Text(item.Quantity.ToString());
                            table.Cell().Element(CellStyle).Text(item.UnitPrice.ToString("C", CultureInfo.CurrentCulture));
                            table.Cell().Element(CellStyle).Text(item.LineTotal.ToString("C", CultureInfo.CurrentCulture));

                            static IContainer CellStyle(IContainer container) =>
                                container.Padding(5);
                        }
                    });

                    col.Item().PaddingTop(10).AlignRight().Text(
                        $"Total: {invoice.Items.Sum(i => i.LineTotal):C}");
                });

                page.Footer()
                    .AlignCenter()
                    .Text(x => x.Span("Generated on " + DateTime.Now.ToString("g")));
                    // .FontSize(10).Italic().FontColor(Colors.Grey.Medium);
            });
        });

        return document.GeneratePdf();
    }
}

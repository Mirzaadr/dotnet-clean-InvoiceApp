using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Products.Create;

public record CreateProductCommand : IRequest
{
    public string Name { get; set; } = "";
    public string? Description { get; set; } = "";
    public double Price { get; set; }
};

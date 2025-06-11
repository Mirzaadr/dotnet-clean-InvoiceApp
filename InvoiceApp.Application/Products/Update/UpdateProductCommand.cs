using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Products.Update;

public record UpdateProductCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; } = "";
    public double Price { get; set; }
};

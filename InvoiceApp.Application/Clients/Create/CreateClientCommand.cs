using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Clients.Create;

public record CreateClientCommand : IRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
};

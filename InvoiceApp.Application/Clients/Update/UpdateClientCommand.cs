using InvoiceApp.Application.DTOs;
using MediatR;

namespace InvoiceApp.Application.Clients.Update;

public record UpdateClientCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
};

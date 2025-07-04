using MediatR;

namespace InvoiceApp.Application.Users.Login;

public record LoginUserCommand(
    string Username,
    string Password
) : IRequest<bool>;
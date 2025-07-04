using InvoiceApp.Application.Commons.Interface;
using InvoiceApp.Domain.Users;
using MediatR;

namespace InvoiceApp.Application.Users.Login;

internal sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _hasher;

    public LoginUserCommandHandler(IUserRepository userRepository, IPasswordHasher hasher)
    {
        _userRepository = userRepository;
        _hasher = hasher;
    }

    public async Task<bool> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // check user exist
        User? user = await _userRepository.GetByUsernameAsync(request.Username);

        if (user is null || !_hasher.Verify(request.Password, user.PasswordHash))
        // if (user is null)
        {
            return false;
        }
        
        return true;
    }
}
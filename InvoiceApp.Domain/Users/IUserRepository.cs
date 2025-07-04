namespace InvoiceApp.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(UserId id);
    Task<User?> GetByUsernameAsync(string username);
}
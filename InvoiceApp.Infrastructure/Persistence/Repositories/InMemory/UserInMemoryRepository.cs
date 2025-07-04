using InvoiceApp.Domain.Users;

namespace InvoiceApp.Infrastructure.Persistence.Repositories.InMemory;

public class UserInMemoryRepository : IUserRepository
{
    private readonly InMemoryDbContext _context;

    public UserInMemoryRepository(InMemoryDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(UserId id)
    {
        var user = _context.Users.FirstOrDefault(i => i.Id == id);
        return await Task.FromResult(user);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        var user = _context.Users.FirstOrDefault(i => i.Username == username);
        return await Task.FromResult(user);
    }

}
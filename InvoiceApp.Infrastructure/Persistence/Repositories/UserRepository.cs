using InvoiceApp.Domain.Commons.Models;
using InvoiceApp.Domain.Invoices;
using InvoiceApp.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace InvoiceApp.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly InMemoryDbContext _context;

    public UserRepository(InMemoryDbContext context)
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
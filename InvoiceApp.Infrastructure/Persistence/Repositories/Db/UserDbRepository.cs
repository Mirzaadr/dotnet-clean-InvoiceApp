using InvoiceApp.Application.Commons.Interface;
using InvoiceApp.Domain.Clients;
using InvoiceApp.Domain.Commons.Models;
using InvoiceApp.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace InvoiceApp.Infrastructure.Persistence.Repositories.Db;

public class UserDbRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public UserDbRepository(AppDbContext context, IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _context = context;
        _dbContextFactory = dbContextFactory;
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
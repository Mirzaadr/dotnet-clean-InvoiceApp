using InvoiceApp.Domain.Commons.Models;

namespace InvoiceApp.Domain.Users;

public class User : BaseEntity<UserId>
{
    public string Username { get; private set; }
    public string PasswordHash { get; private set; }  // Use hash, not plain text!

    public User(
        UserId id,
        string username,
        string passwordHash,
        DateTime? createdDate,
        DateTime? updatedDate
    ): base(id, createdDate, updatedDate)
    {
        Username = username;
        PasswordHash = passwordHash;
    }
    
    public static User Create(string username, string passwordHash)
    {
        return new (
            UserId.New(),
            username,
            passwordHash,
            DateTime.UtcNow,
            DateTime.UtcNow
        );
    }

    // public bool VerifyPassword(string password)
    // {
    //     return PasswordHash == BCrypt.Net.BCrypt.HashPassword(password); // or compare using HashPassword + Verify
    // }
}

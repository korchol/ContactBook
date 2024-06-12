using ContactBook.Domain.Common;
using ErrorOr;

namespace ContactBook.Api.Authentication.PasswordHasher;

public partial class PasswordHasher : IPasswordHasher
{
    public ErrorOr<string> HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    public bool IsCorrectPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}
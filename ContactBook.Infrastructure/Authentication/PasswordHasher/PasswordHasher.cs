using ContactBook.Domain.Common;

namespace ContactBook.Api.Authentication.PasswordHasher;

//implementacja IPasswordHasher przy pomocy bilioteki BCrypt
public partial class PasswordHasher : IPasswordHasher
{
    //zwraca hash stringa
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    }

    //sprawdza czy hash == stringHashed
    public bool IsCorrectPassword(string password, string hash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
    }
}
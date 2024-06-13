using ContactBook.Domain.Common;

namespace ContactBook.Domain.Entities;


//prosta klasa domenowa User
public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }

    private readonly string _passwordHash = null!;

    //metoda sprawdza czy password hash obiektu zgadza się z otrzymanym podczas logowania stringiem
    public bool IsCorrectPasswordHash(string password, IPasswordHasher passwordHasher)
    {
        return passwordHasher.IsCorrectPassword(password, _passwordHash);
    }

    //prosty konstruktor
    public User(string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        Email = email;
        _passwordHash = passwordHash;
    }

    //dla EF
    private User()
    {
    }
}

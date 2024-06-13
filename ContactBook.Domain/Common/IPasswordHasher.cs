namespace ContactBook.Domain.Common;

//z interfejsu korzystamy w klasie User aby zwalidowa� has�o u�ytkownika
public interface IPasswordHasher
{
    public string HashPassword(string password);
    bool IsCorrectPassword(string password, string hash);
}
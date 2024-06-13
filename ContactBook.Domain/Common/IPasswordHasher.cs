namespace ContactBook.Domain.Common;

//z interfejsu korzystamy w klasie User aby zwalidowaæ has³o u¿ytkownika
public interface IPasswordHasher
{
    public string HashPassword(string password);
    bool IsCorrectPassword(string password, string hash);
}
namespace ContactBook.Infrastructure.Authentication.TokenGenerator;

//klasa u�ywana do wstrzykni�cia warto�ci do JwtTokenGenerator z pliku konfiguracyjnego
public class JwtSettings
{
    public const string Section = "JwtSettings";

    public string Audience { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Secret { get; set; } = null!;
    public int TokenExpirationInMinutes { get; set; }
}
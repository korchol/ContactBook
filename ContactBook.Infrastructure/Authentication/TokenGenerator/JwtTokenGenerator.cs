using ContactBook.Application.Common.Interfaces;
using ContactBook.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactBook.Infrastructure.Authentication.TokenGenerator;

//implementacja IJwtTokenGenerator, odpowiada za tworzenie odpowiedniego tokena JWT do autoryzacji
public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateToken(User user)
    {
        //tworzy klucz symetrycznego z tajnego klucza w ustawieniach
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        //tworzy poœwiadczenie do podpisania tokena przy u¿yciu algorytmu HmacSha256
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //definiuje roszczenia
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };

        //tworzy token z odpowiednimi ustaiweniami
        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            null,
            DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
            credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
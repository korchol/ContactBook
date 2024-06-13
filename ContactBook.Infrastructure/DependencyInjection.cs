using ContactBook.Api.Authentication.PasswordHasher;
using ContactBook.Application.Common.Interfaces;
using ContactBook.Domain.Common;
using ContactBook.Infrastructure.Authentication.TokenGenerator;
using ContactBook.Infrastructure.Common;
using ContactBook.Infrastructure.Repositories;
using GymManagement.Infrastructure.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace ContactBook.Infrastructure;

public static class DependencyInjection
{
    //metoda dodająca zależności do kontenera DI
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //konfiguracja bazy danych
        services.AddDbContext<ContactBookDbContext>(options =>
            options.UseSqlServer("Data Source=LAPTOP-O83OGQKR\\SQLEXPRESS;Initial Catalog=ContactBookDb;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"));

        //rejestracja repozytoriów
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<ICategorySetRepository, CategorySetRepository>();

        // rejestracja IUnitOfWork
        services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ContactBookDbContext>());

        //konfiguracja ustawień JWT
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.Section, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        //konfiguracja uwierzytelniania
        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            });

        //zwraca kolekcje usług używaną w warstwie api
        return services;
    }
}

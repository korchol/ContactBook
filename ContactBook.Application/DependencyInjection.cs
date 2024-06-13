using ContactBook.Application.Common.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ContactBook.Application;

public static class DependencyInjection
{
    //metoda dodająca zależności do kontenera DI
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        //rejestracja MediatR
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));

            //dodanie niestandardowego zachowania do pipelinea MediatR
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        //rejestracja walidatorów z FluentValidation
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        //Rejestracja Automapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        //Zwracamy kolekcję usług którą pobieramy w warstwie api
        return services;
    }
}

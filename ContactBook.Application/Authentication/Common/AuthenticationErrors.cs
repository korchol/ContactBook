using ErrorOr;

namespace ContactBook.Application.Authentication.Common;

//Definicja b³êdu niepoprawne dane logowania
public static class AuthenticationErrors
{
    public static readonly Error InvalidCredentials = Error.Validation(
        code: "Authentication.InvalidCredentials",
        description: "Invalid credentials");
}
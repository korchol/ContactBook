using ContactBook.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace ContactBook.Application.Authentication.Commands;

//Definicja parametrów oraz rezultatu komendy "RegisterCommand"
public record RegisterCommand(
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;
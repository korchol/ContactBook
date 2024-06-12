using ContactBook.Application.Authentication.Common;
using ErrorOr;
using MediatR;

namespace ContactBook.Application.Authentication.Queries;

public record LoginQuery(
    string Email,
    string Password) : IRequest<ErrorOr<AuthenticationResult>>;
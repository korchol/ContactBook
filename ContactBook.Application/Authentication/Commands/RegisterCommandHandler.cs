using ContactBook.Application.Authentication.Common;
using ContactBook.Application.Common.Interfaces;
using ContactBook.Domain.Common;
using ContactBook.Domain.Entities;
using ErrorOr;
using MediatR;

namespace ContactBook.Application.Authentication.Commands;

public class RegisterCommandHandler(
    IJwtTokenGenerator _jwtTokenGenerator,
    IPasswordHasher _passwordHasher,
    IUserRepository _usersRepository,
    IUnitOfWork _unitOfWork)
        : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        if (await _usersRepository.ExistsByEmailAsync(command.Email))
        {
            return Error.Conflict(description: "User already exists");
        }

        var hashPasswordResult = _passwordHasher.HashPassword(command.Password);

        if (hashPasswordResult.IsError)
        {
            return hashPasswordResult.Errors;
        }

        var user = new User(
            command.Email,
            hashPasswordResult.Value);

        await _usersRepository.AddUserAsync(user);
        await _unitOfWork.CommitChangesAsync();

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
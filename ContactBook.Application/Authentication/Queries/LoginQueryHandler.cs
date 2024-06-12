using ContactBook.Application.Authentication.Common;
using ContactBook.Application.Common.Interfaces;
using ContactBook.Domain.Common;
using ErrorOr;
using MediatR;

namespace ContactBook.Application.Authentication.Queries;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;

    public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(query.Email);

        if (user is null || !user.IsCorrectPasswordHash(query.Password, _passwordHasher))
        {
            return AuthenticationErrors.InvalidCredentials;
        }
        return new AuthenticationResult(user, _jwtTokenGenerator.GenerateToken(user));
    }
}
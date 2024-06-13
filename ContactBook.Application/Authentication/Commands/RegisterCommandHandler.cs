using ContactBook.Application.Authentication.Common;
using ContactBook.Application.Common.Interfaces;
using ContactBook.Domain.Common;
using ContactBook.Domain.Entities;
using ErrorOr;
using MediatR;

namespace ContactBook.Application.Authentication.Commands;

// Handler dla komendy RegisterCommand
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        //Sprawdza czy uzytkownik ju¿ istnieje
        if (await _userRepository.ExistsByEmailAsync(command.Email))
        {
            //Zwraca b³¹d je¿eli tak
            return Error.Conflict(description: "User already exists");
        }

        //Haszuje has³o
        var hashPasswordResult = _passwordHasher.HashPassword(command.Password);

        //Tworzy obiekt u¿ytkownika
        var user = new User(command.Email, hashPasswordResult);

        //Dodaje do bazy a nastêpnie zapisujemy
        await _userRepository.AddUserAsync(user);
        await _unitOfWork.CommitChangesAsync();

        //Generuje token JWT
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
using FluentValidation;
using System.Text.RegularExpressions;

namespace ContactBook.Application.Authentication.Commands.CreateContact;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    private const string pwRegex = @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{8,16}$";
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email address in invalid.");
        RuleFor(x => x.Password).NotEmpty().Must(p => Regex.IsMatch(p, pwRegex)).WithMessage("Password does not fill password requirements.");
    }
}

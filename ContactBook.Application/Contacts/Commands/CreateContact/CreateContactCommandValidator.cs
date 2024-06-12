using FluentValidation;
using System.Text.RegularExpressions;

namespace ContactBook.Application.Contacts.Commands.CreateContact;

public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
{
    private const string pwRegex = @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{8,16}$";
    private const string phoneRegex = @"^(?:\+1)?\s?\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{3}$";
    public CreateContactCommandValidator()
    {
        RuleFor(x => x.Category).NotEmpty();
        RuleFor(x => x.Subcategory);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().Must(p => Regex.IsMatch(p, pwRegex));
        RuleFor(x => x.Phone).NotEmpty().Must(p => Regex.IsMatch(p, phoneRegex));
        RuleFor(x => x.Birthday).NotEmpty().LessThan(DateOnly.FromDateTime(DateTime.Now));
    }
}
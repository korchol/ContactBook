using ContactBook.Application.Common.Dto;
using ErrorOr;
using MediatR;

namespace ContactBook.Application.Contacts.Commands.UpdateContact;

//Definicja parametrów oraz rezultatu komendy "UpdateContactCommand"
public record UpdateContactCommand(
    Guid Id,
    string Category,
    string Subcategory,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string Phone,
    DateOnly Birthday) : IRequest<ErrorOr<ContactDto>>;

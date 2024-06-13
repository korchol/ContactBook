using ContactBook.Application.Common.Dto;
using ErrorOr;
using MediatR;

namespace ContactBook.Application.Contacts.Queries.GetContact;

//Definicja parametrów oraz rezultatu zapytania "GetContactQuery"
public record GetContactQuery(Guid ContactId) : IRequest<ErrorOr<ContactDto>>;

using ContactBook.Application.Common.Dto;
using ErrorOr;
using MediatR;

namespace ContactBook.Application.Contacts.Queries.ListContacts;

//Definicja parametrów oraz rezultatu zapytania "ListContactQuery"
public record ListContactsQuery() : IRequest<ErrorOr<List<ContactDto>>>;

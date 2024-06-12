using ContactBook.Application.Common.Dto;
using ErrorOr;
using MediatR;

namespace ContactBook.Application.Contacts.Queries.ListContacts;

public record ListContactsQuery() : IRequest<ErrorOr<List<ContactDto>>>;

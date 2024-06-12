using ContactBook.Application.Common.Dto;
using ErrorOr;
using MediatR;

namespace ContactBook.Application.Contacts.Queries.GetContact;

public record GetContactQuery(Guid ContactId) : IRequest<ErrorOr<ContactDto>>;

using ErrorOr;
using MediatR;


namespace ContactBook.Application.Contacts.Commands.DeleteContact;

public record DeleteContactCommand(Guid ContactId) : IRequest<ErrorOr<Deleted>>;

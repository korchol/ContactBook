using ErrorOr;
using MediatR;


namespace ContactBook.Application.Contacts.Commands.DeleteContact;

//Definicja parametrów oraz rezultatu komendy "DeleteContactCommand"
public record DeleteContactCommand(Guid ContactId) : IRequest<ErrorOr<Deleted>>;

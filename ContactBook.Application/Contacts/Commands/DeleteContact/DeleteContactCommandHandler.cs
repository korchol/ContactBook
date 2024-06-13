using ContactBook.Application.Common.Interfaces;
using ContactBook.Domain.Common;
using ErrorOr;
using MediatR;

namespace ContactBook.Application.Contacts.Commands.DeleteContact;

// Handler dla komendy DeleteContactCommand
public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, ErrorOr<Deleted>>
{
    private readonly IContactRepository _contactRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteContactCommandHandler(IContactRepository contactRepository, IUnitOfWork unitOfWork)
    {
        _contactRepository = contactRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        //fetchuje kontakt z bazy danych
        var contact = await _contactRepository.GetByIdAsync(request.ContactId);
        if (contact == null)
        {
            //jeżeli nie isnieje zwraca NotFound
            return DomainErrors.Contact.NotFound(request.ContactId);
        }

        
        await _contactRepository.RemoveAsync(contact);

        await _unitOfWork.CommitChangesAsync();

        return Result.Deleted;
    }
}

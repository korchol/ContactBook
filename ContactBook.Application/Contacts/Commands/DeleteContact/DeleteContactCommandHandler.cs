using ContactBook.Application.Common.Interfaces;
using ErrorOr;
using MediatR;

namespace ContactBook.Application.Contacts.Commands.DeleteContact;

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
        var contact = await _contactRepository.GetByIdAsync(request.ContactId);
        if (contact == null)
        {
            return Error.NotFound(description: "Contact not found.");
        }

        await _contactRepository.RemoveAsync(contact);

        await _unitOfWork.CommitChangesAsync();

        return Result.Deleted;
    }
}

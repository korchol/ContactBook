using AutoMapper;
using ContactBook.Application.Common.Dto;
using ContactBook.Application.Common.Interfaces;
using ErrorOr;
using MediatR;

namespace ContactBook.Application.Contacts.Queries.ListContacts;

public class ListContactsQueryHandler : IRequestHandler<ListContactsQuery, ErrorOr<List<ContactDto>>>
{
    private readonly IContactRepository _contactRepository;
    private readonly IMapper _mapper;
    public ListContactsQueryHandler(IContactRepository contactRepository, IMapper mapper)
    {
        _contactRepository = contactRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<List<ContactDto>>> Handle(ListContactsQuery request, CancellationToken cancellationToken)
    {
        var contacts = await _contactRepository.ListContactsAsync();

        if (contacts is null)
        {
            return Error.NotFound(description: "No contacts found.");
        }

        var contactDtos = _mapper.Map<List<ContactDto>>(contacts);
        return contactDtos;
    }
}

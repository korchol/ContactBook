using AutoMapper;
using ContactBook.Application.Common.Dto;
using ContactBook.Application.Common.Interfaces;
using ErrorOr;
using MediatR;

namespace ContactBook.Application.Contacts.Queries.GetContact;

public class GetContactQueryHandler : IRequestHandler<GetContactQuery, ErrorOr<ContactDto>>
{
    private readonly IContactRepository _contactRepository;
    private readonly ICategorySetRepository _categorySetRepository;
    private readonly IMapper _mapper;

    public GetContactQueryHandler(IContactRepository contactRepository, IMapper mapper, ICategorySetRepository categorySetRepository)
    {
        _contactRepository = contactRepository;
        _mapper = mapper;
        _categorySetRepository = categorySetRepository;
    }

    public async Task<ErrorOr<ContactDto>> Handle(GetContactQuery query, CancellationToken cancellationToken)
    {
        //sprawdza czy kontakt istnieje
        var contact = await _contactRepository.GetByIdAsync(query.ContactId);
        if (contact is null)
        {
            return Error.NotFound(description: "Contact not found");
        }

        //pobiera też zestaw kategorii aby umożliwić mapperowi konwersję do nazw kategorii
        var categorySet = await _categorySetRepository.GetByIdAsync(contact.CategorySetId);

        //konwertuje do ładnego obiektu i zwraca do prezentacji
        var contactDto = _mapper.Map<ContactDto>(contact);
        return contactDto;
    }
}
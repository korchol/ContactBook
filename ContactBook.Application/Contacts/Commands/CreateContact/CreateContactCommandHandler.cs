using AutoMapper;
using ContactBook.Application.Common.Dto;
using ContactBook.Application.Common.Interfaces;
using ContactBook.Domain.Common;
using ContactBook.Domain.Entities;
using ErrorOr;
using MediatR;

namespace ContactBook.Application.Contacts.Commands.CreateContact;

// Handler dla komendy CreateContactCommand
public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, ErrorOr<ContactDto>>
{
    private readonly IContactRepository _contactRepository;
    private readonly ICategorySetRepository _categorySetRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateContactCommandHandler(IContactRepository contactRepository, ICategorySetRepository categorySetRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _contactRepository = contactRepository;
        _categorySetRepository = categorySetRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<ContactDto>> Handle(CreateContactCommand command, CancellationToken cancellationToken)
    {
        //sprawdza czy zestaw kategorii jest dostęony w bazie
        var categorySet = await _categorySetRepository.GetBySetAsync(command.Category, command.Subcategory);
        if (categorySet is null)
        {
            //jeżeli nie, tworzy objekt zestaw kategorii
            var createCategorySetResult = CategorySet.CreateCustomCategorySet(command.Category, command.Subcategory);
            if (createCategorySetResult.IsError)
            {   
                //zwraca error jeżeli kategoria główna != Custom lub null
                //wg logiki biznesowej możemy korzystać z zestawów dostępnych w bazie danych LUB utworzyć nowy zestaw ale tylko z kategorią Custom
                return createCategorySetResult.Errors;
            }
            categorySet = createCategorySetResult.Value;
            await _categorySetRepository.AddSetAsync(categorySet);
        }

        //sprawdza czy email istnieje w bazce
        var emailTaken = await _contactRepository.EmailTaken(command.Email);
        if (emailTaken)
        {
            //istnieje - error
            return DomainErrors.Contact.EmailAlreadyExists(command.Email);
        }

        //próba stworzenia User z dostępnych danych, obiekt w założeniu jest Allways Valid i może zwrócić error, ale nie zaimplementowałem odpowiedniej walidacji w domenie
        //więc kod niżej jest tylko placeholderem
        var createContactResult = Contact.Create(categorySet.Id, command.FirstName, command.LastName, command.Email, command.Password, command.Phone, command.Birthday);
        if (createContactResult.IsError)
        {
            //jeżeli zakończona niepowodzeniem, zwracamy otrzymany Error
            return createContactResult.Errors;
        }

        //dodaje i zapisuje korzystając z Unit of Work pattern aby nasz zapis do bazy napewno był atomiczny i pełny
        await _contactRepository.AddContactAsync(createContactResult.Value);
        await _unitOfWork.CommitChangesAsync();

        //mapuje do ładnego obiektu dla warstwy prezentacji
        return _mapper.Map<ContactDto>(createContactResult.Value);
    }
}
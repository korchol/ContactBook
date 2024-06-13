using AutoMapper;
using ContactBook.Application.Common.Dto;
using ContactBook.Application.Common.Interfaces;
using ContactBook.Domain.Common;
using ContactBook.Domain.Entities;
using ErrorOr;
using MediatR;

namespace ContactBook.Application.Contacts.Commands.UpdateContact;

// Handler dla komendy UpdateContactCommand
public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, ErrorOr<ContactDto>>
{
    private readonly IContactRepository _contactRepository;
    private readonly ICategorySetRepository _categorySetRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateContactCommandHandler(IContactRepository contactRepository, ICategorySetRepository categorySetRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _contactRepository = contactRepository;
        _categorySetRepository = categorySetRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<ContactDto>> Handle(UpdateContactCommand command, CancellationToken cancellationToken)
    {
        //sprawdza czy zestaw kategorii jest dostęony w bazie
        var contact = await _contactRepository.GetByIdAsync(command.Id);
        if (contact is null)
        {
            return DomainErrors.Contact.NotFound(command.Id);
        }

        //sprawdza czy zestaw kategorii jest dostęony w bazie
        var categorySet = await _categorySetRepository.GetBySetAsync(command.Category, command.Subcategory);
        Guid categoryId = categorySet != null ? categorySet.Id : Guid.NewGuid();
        if (categorySet is null)
        {
            //jeżeli nie, tworzy objekt zestaw kategorii
            var createCategoryResult = CategorySet.CreateCustomCategorySet(command.Category, command.Subcategory);
            if (createCategoryResult.IsError)
            {
                //zwraca error jeżeli kategoria główna != Custom lub null
                //wg logiki biznesowej możemy korzystać z zestawów dostępnych w bazie danych LUB utworzyć nowy zestaw ale tylko z kategorią Custom
                return createCategoryResult.Errors;
            }
            await _categorySetRepository.AddSetAsync(createCategoryResult.Value);
            categoryId = createCategoryResult.Value.Id;
        }

        //próba aktualizacji User z dostępnych danych, obiekt w założeniu jest Allways Valid i może zwrócić error, ale nie zaimplementowałem odpowiedniej walidacji w domenie
        //więc kod niżej jest tylko placeholderem
        var updateContactResult = contact.UpdateDetails(categoryId, command.FirstName, command.LastName, command.Email, command.Password, command.Phone, command.Birthday);
        if (updateContactResult.IsError)
        {
            return updateContactResult.Errors;
        }

        await _unitOfWork.CommitChangesAsync();

        return _mapper.Map<ContactDto>(contact);
    }
}

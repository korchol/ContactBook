using AutoMapper;
using ContactBook.Application.Common.Dto;
using ContactBook.Application.Common.Interfaces;
using ContactBook.Domain.Common;
using ContactBook.Domain.Entities;
using ErrorOr;
using MediatR;

namespace ContactBook.Application.Contacts.Commands.CreateContact;

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
        var categorySet = await _categorySetRepository.GetBySetAsync(command.Category, command.Subcategory);
        if (categorySet is null)
        {
            var createCategorySetResult = CategorySet.CreateCustomCategorySet(command.Category, command.Subcategory);
            if (createCategorySetResult.IsError)
            {
                return createCategorySetResult.Errors;
            }
            categorySet = createCategorySetResult.Value;
            await _categorySetRepository.AddSetAsync(categorySet);
        }

        var emailTaken = await _contactRepository.EmailTaken(command.Email);
        if (emailTaken)
        {
            return DomainErrors.Contact.EmailAlreadyExists(command.Email);
        }

        var createContactResult = Contact.Create(categorySet.Id, command.FirstName, command.LastName, command.Email, command.Password, command.Phone, command.Birthday);
        if (createContactResult.IsError)
        {
            return createContactResult.Errors;
        }

        await _contactRepository.AddContactAsync(createContactResult.Value);
        await _unitOfWork.CommitChangesAsync();

        return _mapper.Map<ContactDto>(createContactResult.Value);
    }
}
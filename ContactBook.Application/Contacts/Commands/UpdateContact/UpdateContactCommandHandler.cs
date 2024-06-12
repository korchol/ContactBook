using AutoMapper;
using ContactBook.Application.Common.Dto;
using ContactBook.Application.Common.Interfaces;
using ContactBook.Domain.Common;
using ContactBook.Domain.Entities;
using ErrorOr;
using MediatR;

namespace ContactBook.Application.Contacts.Commands.UpdateContact;

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
        var contact = await _contactRepository.GetByIdAsync(command.Id);
        if (contact is null)
        {
            return DomainErrors.Contact.ContactNotFound(command.Id);
        }

        var categorySet = await _categorySetRepository.GetBySetAsync(command.Category, command.Subcategory);
        Guid categoryId = categorySet.Id;
        if (categorySet is null)
        {
            var createCategoryResult = CategorySet.CreateCustomCategorySet(command.Category, command.Category);
            if (createCategoryResult.IsError)
            {
                return createCategoryResult.Errors;
            }
            await _categorySetRepository.AddSetAsync(createCategoryResult.Value);
            categoryId = createCategoryResult.Value.Id;
        }

        var updateContactResult = contact.UpdateDetails(categoryId, command.FirstName, command.LastName, command.Email, command.Password, command.Phone, command.Birthday);

        if (updateContactResult.IsError)
        {
            return updateContactResult.Errors;
        }

        await _unitOfWork.CommitChangesAsync();

        return _mapper.Map<ContactDto>(contact);
    }
}

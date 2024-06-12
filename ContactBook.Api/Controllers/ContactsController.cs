using ContactBook.Application.Contacts.Commands.CreateContact;
using ContactBook.Application.Contacts.Commands.DeleteContact;
using ContactBook.Application.Contacts.Commands.UpdateContact;
using ContactBook.Application.Contacts.Queries.GetContact;
using ContactBook.Application.Contacts.Queries.ListContacts;
using ContactBook.Contracts.Contacts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactBook.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactsController : ApiController
{
    private readonly IMediator _mediator;
    public ContactsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateContact(CreateContactRequest request)
    {
        var subcategory = request.Subcategory ?? string.Empty;

        var command = new CreateContactCommand(
            request.Category,
            subcategory,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            request.Phone,
            request.Birthday);

        var createContactResult = await _mediator.Send(command);

        return createContactResult.MatchFirst(
            contact => Ok(new ContactDetailedResponse(
                contact.Id,
                contact.CategoryName,
                contact.SubcategoryName,
                contact.FirstName,
                contact.LastName,
                contact.Email,
                contact.Password,
                contact.Phone,
                contact.Birthday)),
            Problem);
    }

    [Authorize]
    [HttpPut("{contactId:guid}")]
    public async Task<IActionResult> UpdateContact(Guid contactId, UpdateContactRequest request)
    {
        var subcategory = request.Subcategory ?? string.Empty;

        var command = new UpdateContactCommand(
            contactId,
            request.Category,
            subcategory,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            request.Phone,
            request.Birthday);

        var createContactResult = await _mediator.Send(command);

        return createContactResult.MatchFirst(
            contact => Ok(new ContactDetailedResponse(
                contact.Id,
                contact.CategoryName,
                contact.SubcategoryName,
                contact.FirstName,
                contact.LastName,
                contact.Email,
                contact.Password,
                contact.Phone,
                contact.Birthday)),
            Problem);
    }

    [HttpGet("{contactId:guid}")]
    public async Task<IActionResult> GetContact(Guid contactId)
    {
        var query = new GetContactQuery(contactId);

        var getContactResult = await _mediator.Send(query);

        return getContactResult.MatchFirst(
            contact => Ok(new ContactDetailedResponse(
                contact.Id,
                contact.CategoryName,
                contact.SubcategoryName,
                contact.FirstName,
                contact.LastName,
                contact.Email,
                contact.Password,
                contact.Phone,
                contact.Birthday)),
            Problem);
    }

    [Authorize]
    [HttpDelete("{contactId:guid}")]
    public async Task<IActionResult> DeleteContact(Guid contactId)
    {
        var command = new DeleteContactCommand(contactId);

        var deleteContactResult = await _mediator.Send(command);

        return deleteContactResult.Match(
            _ => Ok(),
            Problem);
    }

    [HttpGet]
    public async Task<IActionResult> ListContacts()
    {
        var query = new ListContactsQuery();

        var getContactListResult = await _mediator.Send(query);

        return getContactListResult.MatchFirst(
            contacts => Ok(contacts.ConvertAll(
                contact => new ContactSummaryResponse(
                    contact.Id,
                    contact.CategoryName,
                    contact.FirstName,
                    contact.Phone))),
            Problem);
    }
}
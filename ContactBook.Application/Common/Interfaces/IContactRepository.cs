using ContactBook.Domain.Entities;

namespace ContactBook.Application.Common.Interfaces;

public interface IContactRepository
{
    Task AddContactAsync(Contact contact);
    Task<Contact?> GetByIdAsync(Guid contactId);
    Task<List<Contact>?> ListContactsAsync();
    Task RemoveAsync(Contact contact);
    Task<bool> EmailTaken(string email);
}

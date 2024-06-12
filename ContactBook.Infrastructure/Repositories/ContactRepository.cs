﻿using ContactBook.Application.Common.Interfaces;
using ContactBook.Domain.Entities;
using ContactBook.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.Infrastructure.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly ContactBookDbContext _dbContext;

    public async Task AddContactAsync(Contact contact)
    {
        await _dbContext.Contacts.AddAsync(contact);
    }

    public async Task<bool> EmailTaken(string email)
    {
        return await _dbContext.Contacts.AnyAsync(c => c.Email == email);
    }

    public async Task<Contact?> GetByIdAsync(Guid contactId)
    {
        var contact = await _dbContext.Contacts.FindAsync(contactId);
        return contact;
    }

    public async Task<List<Contact>?> ListContactsAsync()
    {
        var contacts = await _dbContext.Contacts.ToListAsync();
        return contacts;
    }

    public async Task RemoveAsync(Contact contact)
    {
        _dbContext.Contacts.Remove(contact);
    }
}

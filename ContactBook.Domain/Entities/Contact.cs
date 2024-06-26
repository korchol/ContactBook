﻿using ErrorOr;

namespace ContactBook.Domain.Entities;

//prosta klasa domenowa contact, zawiera właściwości i kontaktu oraz referuje do klasy CategorySet
public class Contact
{
    public Guid Id { get; private set; }
    public Guid CategorySetId { get; private set; }
    public CategorySet? CategorySet { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public string Phone { get; private set; }
    public DateOnly Birthday { get; private set; }

    private Contact(
        Guid id,
        Guid categorySetId,
        string firstName,
        string lastName,
        string email,
        string password,
        string phone,
        DateOnly birthday)
    {
        Id = id;
        CategorySetId = categorySetId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Phone = phone;
        Birthday = birthday;
    }

    //kreator obiektu, zazwyczaj posiada dodatkową logikę, walidację, utworzenie eventu itd.
    public static ErrorOr<Contact> Create(
        Guid categorySetId,
        string firstName,
        string lastName,
        string email,
        string password,
        string phone,
        DateOnly birthday)
    {
        var contact = new Contact(Guid.NewGuid(), categorySetId, firstName, lastName, email, password, phone, birthday);

        return contact;
    }

    //metoda aktualizuje obiekt
    public ErrorOr<Success> UpdateDetails(
        Guid categorySetId,
        string firstName,
        string lastName,
        string email,
        string password,
        string phone,
        DateOnly birthday)
    {
        CategorySetId = categorySetId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Phone = phone;
        Birthday = birthday;

        return new Success();
    }
}

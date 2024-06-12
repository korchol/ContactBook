namespace ContactBook.Contracts.Contacts;

public record CreateContactRequest(string Category, string? Subcategory, string FirstName, string LastName, string Email, string Password, string Phone, DateOnly Birthday);

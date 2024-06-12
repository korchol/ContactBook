namespace ContactBook.Contracts.Contacts;

public record ContactSummaryResponse(Guid Id, string Category, string FirstName, string Phone);

namespace ContactBook.Application.Common.Dto;

//Definicja DTO który będziemy zwracać do warstwy prezentacji, nie chcemy Id kategorii, chcemy kategorie i podkategorię jako string
public class ContactDto
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; }
    public string SubcategoryName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public DateOnly Birthday { get; set; }
}

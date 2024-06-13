using ContactBook.Domain.Common;
using ErrorOr;
using System.ComponentModel.DataAnnotations;

namespace ContactBook.Domain.Entities;

//prosta klasa domenowa CategorySet, przechowuje kategorię i podkategorię, sprawdza czy utworzenie zestawu jest możliwe
public class CategorySet
{
    [Key]
    public Guid Id { get; set; }
    public string Category { get; set; }
    public string Subcategory { get; set; }
    private CategorySet(string category, string subcategory)
    {
        Id = Guid.NewGuid();
        Category = category;
        Subcategory = subcategory;
    }

    //Użytkownik może stworzyć nowy zestaw kategorii TYLKO w przypadku kategorii głównej Custom
    public static ErrorOr<CategorySet> CreateCustomCategorySet(string category, string subcategory)
    {
        if (category == null || subcategory == null || category.ToLower() != "custom")
        {
            return DomainErrors.CategorySet.InvalidCategorySet(category, subcategory);
        }
        return new CategorySet(category, subcategory);
    }
}

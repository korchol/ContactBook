using ContactBook.Domain.Common;
using ErrorOr;
using System.ComponentModel.DataAnnotations;

namespace ContactBook.Domain.Entities;

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

    //Użytkownik może stworzyć nowy zestaw kategorii TYLKO w przypadku kategorii głównej Custom.
    public static ErrorOr<CategorySet> CreateCustomCategorySet(string category, string subcategory)
    {
        if (category == null || subcategory == null || category != "Custom")
        {
            return DomainErrors.CategorySet.InvalidCategorySet(category, subcategory);
        }
        return new CategorySet(category, subcategory);
    }
}

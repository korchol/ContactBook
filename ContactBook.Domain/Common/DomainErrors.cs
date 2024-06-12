using ErrorOr;

namespace ContactBook.Domain.Common;

public static class DomainErrors
{
    public static class Contact
    {
        public static Error ContactNotFound(Guid userId)
        {
            return Error.NotFound(
            code: "NotFound",
            description: $"Id '{userId}' does not exist.");
        }
        public static Error EmailAlreadyExists(string email)
        {
            return Error.Conflict(
            code: "EmailExists",
            description: $"Email '{email}' already exists.");
        }
    }

    public static class CategorySet
    {
        public static Error InvalidCategorySet(string category, string subcategory)
        {
            if (category == null || subcategory == null)
            {
                return Error.Validation(
            code: "Category.InvalidSubcategoryInSet",
            description: $"Subcategory and category cannot be null. Use empty string instead");
            }

            return Error.Validation(
            code: "Category.InvalidCategorySet",
            description: $"Contact with '{category}' category cannot receive '{subcategory}' subcategory.");
        }
    }
}

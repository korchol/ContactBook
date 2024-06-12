using ContactBook.Domain.Entities;

namespace ContactBook.Application.Common.Interfaces;

public interface ICategorySetRepository
{
    Task<CategorySet> GetBySetAsync(string category, string subcategory);
    Task AddSetAsync(CategorySet categorySet);
    Task<CategorySet> GetByIdAsync(Guid categorySetId);
}

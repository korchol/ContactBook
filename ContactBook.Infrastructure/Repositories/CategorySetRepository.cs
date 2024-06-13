using ContactBook.Application.Common.Interfaces;
using ContactBook.Domain.Entities;
using ContactBook.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace ContactBook.Infrastructure.Repositories;

//implementacja intefejsu repozytoria CategorySetRepo, odpowiada za kontact z bazą
public class CategorySetRepository : ICategorySetRepository
{
    private readonly ContactBookDbContext _dbContext;

    public CategorySetRepository(ContactBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddSetAsync(CategorySet categorySet)
    {
        await _dbContext.CategorySets.AddAsync(categorySet);
    }

    public async Task<bool> CategoryExistAsync(string categoryName)
    {
        return await _dbContext.CategorySets.AnyAsync(cs => cs.Category == categoryName);
    }

    public async Task<bool> CategoryExistAsync(Guid categoryId)
    {
        return await _dbContext.CategorySets.AnyAsync(cs => cs.Id == categoryId);
    }

    public async Task<CategorySet> GetByIdAsync(Guid categorySetId)
    {
        return await _dbContext.CategorySets.FindAsync(categorySetId);
    }

    public async Task<CategorySet?> GetBySetAsync(string categoryName, string subcategoryName)
    {
        return await _dbContext.CategorySets.SingleOrDefaultAsync(
            cs => cs.Category == categoryName && cs.Subcategory == subcategoryName);
    }

    public async Task<CategorySet?> GetBySetAsync(Guid categoryId, Guid subcategoryId)
    {
        return await _dbContext.CategorySets.SingleOrDefaultAsync(
            cs => cs.Id == categoryId && cs.Id == subcategoryId);
    }
}

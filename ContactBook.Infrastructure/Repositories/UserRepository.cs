using ContactBook.Application.Common.Interfaces;
using ContactBook.Domain.Entities;
using ContactBook.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
namespace GymManagement.Infrastructure.Users;

//implementacja intefejsu repozytoria UserRepository, odpowiada za kontact z bazą
public class UserRepository : IUserRepository
{
    private readonly ContactBookDbContext _dbContext;

    public UserRepository(ContactBookDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddUserAsync(User user)
    {
        await _dbContext.AddAsync(user);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _dbContext.Users.AnyAsync(user => user.Email == email);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
    }
}

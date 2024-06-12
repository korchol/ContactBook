using ContactBook.Domain.Entities;

namespace ContactBook.Application.Common.Interfaces;

public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<bool> ExistsByEmailAsync(string email);
    Task<User?> GetByEmailAsync(string email);
}
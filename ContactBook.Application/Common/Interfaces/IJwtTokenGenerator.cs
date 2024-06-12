using ContactBook.Domain.Entities;

namespace ContactBook.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
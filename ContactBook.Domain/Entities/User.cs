using ContactBook.Domain.Common;

namespace ContactBook.Domain.Entities

{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }

        private readonly string _passwordHash = null!;

        public bool IsCorrectPasswordHash(string password, IPasswordHasher passwordHasher)
        {
            return passwordHasher.IsCorrectPassword(password, _passwordHash);
        }

        public User(string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            Email = email;
            _passwordHash = passwordHash;
        }
        private User()
        {
        }
    }
}

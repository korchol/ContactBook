using ContactBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.Infrastructure.Users;

//klasa odpowiedzialna za mapowanie obiektów do bazy danych przy użyciu EF
public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email);

        builder.Property("_passwordHash")
            .HasColumnName("PasswordHash");
    }
}

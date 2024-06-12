using ContactBook.Application.Common.Interfaces;
using ContactBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ContactBook.Infrastructure.Common;

public class ContactBookDbContext : DbContext, IUnitOfWork
{
    public DbSet<CategorySet> CategorySets { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<User> Users { get; set; }

    public ContactBookDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public async Task CommitChangesAsync()
    {
        await base.SaveChangesAsync();
    }
}

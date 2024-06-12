namespace ContactBook.Application.Common.Interfaces;

public interface IUnitOfWork
{
    Task CommitChangesAsync();
}

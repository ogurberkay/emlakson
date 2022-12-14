namespace DataAccess.Repositories.Abstract;

public interface IUnitOfWork : IAsyncDisposable
{
    IAdvertRepository Adverts { get;}
    Task<int> SaveChangesAsync();
    int SaveChanges();
}
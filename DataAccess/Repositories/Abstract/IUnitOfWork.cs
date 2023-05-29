using Data.Common;

namespace DataAccess.Repositories.Abstract;

public interface IUnitOfWork : IAsyncDisposable
{
    IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity;
    IAdvertRepository Adverts { get;}
    Task<int> SaveChangesAsync();
    int SaveChanges();
}
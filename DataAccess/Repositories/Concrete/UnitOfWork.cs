using DataAccess.Context;
using DataAccess.Repositories.Abstract;

namespace DataAccess.Repositories.Concrete;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private AdvertRepository _advertRepository;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IAdvertRepository Adverts => _advertRepository ??= new AdvertRepository(_context);
    
    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}
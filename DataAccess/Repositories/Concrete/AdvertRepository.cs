using Data.Entities.Models;
using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Concrete;

public class AdvertRepository : GenericRepository<Advert>,IAdvertRepository
{
    public AdvertRepository(DbContext applicationDbContext) : base(applicationDbContext)
    {
    }
    
    DbSet<Advert> Adverts { get; set; }

    /*
     * Additional repo code for advert
     */
    
}
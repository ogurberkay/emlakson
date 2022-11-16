using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = "Server=localhost;Port=5432;Database=test;User Id=postgres;Password=123123;";
        builder.UseNpgsql(connectionString);
        return new ApplicationDbContext(builder.Options);
    }
}
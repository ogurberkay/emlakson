using Microsoft.EntityFrameworkCore;

namespace Core.Extensions;

public static class EnumerableExtensions
{
    public static IQueryable<T> IncludeAll<T>(this IQueryable<T> queryable, params string[] includeProperties)
        where T : class
    {
        foreach (var includeProperty in includeProperties)
            queryable = queryable.Include(includeProperty);
        return queryable;
    }
}
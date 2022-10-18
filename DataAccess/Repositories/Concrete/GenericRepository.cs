using System.Linq.Expressions;
using Core.Extensions;
using Data.Common;
using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Concrete;

public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class,IEntity
{
        protected readonly DbContext _context;
        private readonly DbSet<TEntity> _entitySet;

        public GenericRepository(DbContext context)
        {
            _context = context;

            _entitySet = _context.Set<TEntity>();
        }

        public async Task InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            await _entitySet.AddRangeAsync(entities);
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _entitySet.AddAsync(entity);
            return entity;
        }

        public TEntity Insert(TEntity entity)
        {
            _entitySet.Add(entity);
            return entity;
        }

        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<TEntity> GetByIdAsync(int? id, params string[] includeParams)
        {
            return await _baseQuery(includeParams).FirstOrDefaultAsync(q => q.Id == id) ?? throw new InvalidOperationException();
        }

        public async Task<TEntity> GetAsync(params string[] includeParams)
        {
            return await _baseQuery(includeParams).FirstOrDefaultAsync() ?? throw new InvalidOperationException();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, params string[] includeParams)
        {
            return await _baseQuery(includeParams).FirstOrDefaultAsync(expression) ?? throw new InvalidOperationException();
        }

        public IQueryable<TEntity> FindBy(params string[] includeParams)
        {
            return _baseQuery(includeParams);
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> expression, params string[] includeParams)
        {
            return _baseQuery(includeParams).Where(expression);
        }

        private IQueryable<TEntity> _baseQuery(params string[] includeParams)
        {
            return _entitySet.IncludeAll(includeParams);
        }

        public bool Delete(TEntity entity)
        {
            if (entity is null)
                return false;
            entity.IsDeleted = true;
            Update(entity);
            return true;
        }
}
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fixawy.Repositories
{
    public class Reposatory<T> : IReposatory<T> where T : class
    {
        private readonly ApplicationDBContext _dBContext;
        public DbSet<T> _dbSet;
        public Reposatory(ApplicationDBContext dBContext)
        {
            _dBContext = dBContext;
            _dbSet = _dBContext.Set<T>();
        }

        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
        {
            var entities = await _dbSet.AddAsync(entity, cancellationToken);

            return entities.Entity;
        }

        public void Update(T entity) => _dbSet.Update(entity);

        public void Remove(T entity) => _dbSet.Remove(entity);

        public async Task<IEnumerable<T>> GetAsync
            (
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, Object>>[]? include = null,
            bool Tracking = true,
            CancellationToken cancellationToken = default
            )
        {
            var entities = _dbSet.AsQueryable();

            if (expression is not null)
                entities = entities.Where(expression);

            if (include is not null)
            {
                foreach (var entity in include)
                {
                    entities = entities.Include(entity);
                }
            }

            if (!Tracking)
                entities = entities.AsNoTracking();

            return await entities.ToListAsync(cancellationToken);
        }

        public async Task<T?> GetOneAsync
            (
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, Object>>[]? include = null,
            bool Tracking = true,
            CancellationToken cancellationToken = default
            )
        {
            var query = _dbSet.AsQueryable();

            if (expression is not null)
                query = query.Where(expression);

            if (include is not null)
            {
                foreach (var row in include)
                {
                    query = query.Include(row);
                }
            }

            if (!Tracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IQueryable<T>> GetQueryable
           (
           Expression<Func<T, bool>>? expression = null,
           Expression<Func<T, Object>>[]? include = null,
           bool Tracking = true,
           CancellationToken cancellationToken = default
           )
        {
            var entities = _dbSet.AsQueryable();

            if (expression is not null)
                entities = entities.Where(expression);

            if (include is not null)
            {
                foreach (var entity in include)
                {
                    entities = entities.Include(entity);
                }
            }

            if (!Tracking)
                entities = entities.AsNoTracking();

            return  entities;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Cashier.Application.Interface
{
    public interface IRepository<T> where T : class
    {
        public Task<T> CreateAsync(T entity, CancellationToken cancellationToken);
        public void Update(T entity);

        public void Remove(T entity);

        public Task<IEnumerable<T>> GetAsync
            (
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, Object>>[]? include = null,
            bool Tracking = true,
            CancellationToken cancellationToken = default
            );

        public Task<T?> GetOneAsync
           (
           Expression<Func<T, bool>>? expression = null,
           Expression<Func<T, Object>>[]? include = null,
           bool Tracking = true,
           CancellationToken cancellationToken = default
           );

        public Task<IQueryable<T>> GetQueryable
          (
          Expression<Func<T, bool>>? expression = null,
          Expression<Func<T, Object>>[]? include = null,
          bool Tracking = true,
          CancellationToken cancellationToken = default
          );
    }
}

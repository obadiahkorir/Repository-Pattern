using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RepositoryPattern.Interfaces
{
    public interface IGenericService
    {
        public interface IGenericService
        {
            void AddRange<TEntity>(IEnumerable<TEntity> entities)
                where TEntity : class;

            int Add<TEntity>(TEntity entity)
                where TEntity : class;

            void Update<TEntity>(TEntity entity)
                where TEntity : class;

            void Delete<TEntity>(object id)
                where TEntity : class;

            void Delete<TEntity>(TEntity entity)
                where TEntity : class;

            void DeleteRange<TEntity>(IEnumerable<TEntity> entities)
                where TEntity : class;

            IEnumerable<TEntity> Get<TEntity>(
                Expression<Func<TEntity, bool>> filter = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                string includeProperties = null,
                int? skip = null,
                int? take = null)
                where TEntity : class;

            IEnumerable<TEntity> GetAll<TEntity>(
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                string includeProperties = null,
                int? skip = null,
                int? take = null)
                where TEntity : class;

            Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                string includeProperties = null,
                int? skip = null,
                int? take = null)
                where TEntity : class;

            Task<IEnumerable<TEntity>> GetAsync<TEntity>(
                Expression<Func<TEntity, bool>> filter = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                string includeProperties = null,
                int? skip = null,
                int? take = null)
                where TEntity : class;

            TEntity GetById<TEntity>(object id)
                where TEntity : class;

            Task<TEntity> GetByIdAsync<TEntity>(object id)
                where TEntity : class;
            void Save();

            Task SaveAsync();

            void Dispose();
        }
    }
}


using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryPattern.Data;
using RepositoryPattern.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RepositoryPattern.Services
{
    public class GenericRepository<TContext> : IGenericRepository
        where TContext : DbContext
    {
        public readonly TContext Context;

        public GenericRepository(TContext context)
        {
            Context = context;
        }
        public void AddRange<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public virtual void Add<TEntity>(TEntity entity)
            where TEntity : class
        {
            Context.Set<TEntity>().Add(entity);
        }

        public virtual void Update<TEntity>(TEntity entity)
            where TEntity : class
        {
            Context.Set<TEntity>().Update(entity);
        }
        public virtual void Delete<TEntity>(object id)
            where TEntity : class
        {
            var entity = Context.Set<TEntity>().Find(id);
            Delete(entity);
        }

        public virtual void Delete<TEntity>(TEntity entity)
            where TEntity : class
        {
            var dbSet = Context.Set<TEntity>();
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }

            dbSet.Remove(entity);
        }

        public virtual IEnumerable<TEntity> Get<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class
        {
            return GetQueryable<TEntity>(filter, orderBy, includeProperties, skip, take).ToList();
        }

        public virtual IEnumerable<TEntity> GetAll<TEntity>(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class
        {
            return GetQueryable<TEntity>(null, orderBy, includeProperties, skip, take).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class
        {
            return await GetQueryable<TEntity>(null, orderBy, includeProperties, skip, take).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class
        {
            return await GetQueryable<TEntity>(filter, orderBy, includeProperties, skip, take).ToListAsync();
        }


        public virtual async Task<IQueryable<TEntity>> GetSearchableQueryable<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class
        {

            //make it awaitable
            await Task.FromResult(0);
            return GetQueryable<TEntity>(filter, orderBy, includeProperties, skip, take);
        }

        public virtual TEntity GetById<TEntity>(object id)
            where TEntity : class
        {
            return Context.Set<TEntity>().Find(id);
        }

        public virtual ValueTask<TEntity> GetByIdAsync<TEntity>(object id)
            where TEntity : class
        {
            return Context.Set<TEntity>().FindAsync(id);
        }

        public virtual int GetCount<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class
        {
            return GetQueryable<TEntity>(filter).Count();
        }

        public virtual Task<int> GetCountAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class
        {
            return GetQueryable<TEntity>(filter).CountAsync();
        }

        public virtual bool GetExists<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class
        {
            return GetQueryable<TEntity>(filter).Any();
        }

        public virtual Task<bool> GetExistsAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null)
            where TEntity : class
        {
            return GetQueryable<TEntity>(filter).AnyAsync();
        }

        public virtual TEntity GetFirst<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
            where TEntity : class
        {
            return GetQueryable<TEntity>(filter, orderBy, includeProperties).FirstOrDefault();
        }

        public virtual async Task<TEntity> GetFirstAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null)
            where TEntity : class
        {
            return await GetQueryable(filter, orderBy, includeProperties).FirstOrDefaultAsync().ConfigureAwait(true);
        }


        public virtual TEntity GetOne<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "")
            where TEntity : class
        {
            return GetQueryable<TEntity>(filter, null, includeProperties).SingleOrDefault();
        }

        public virtual Task<TEntity> GetOneAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = null)
            where TEntity : class
        {
            return GetQueryable(filter, null, includeProperties).SingleOrDefaultAsync();
        }

        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public virtual int Save()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (Exception e)
            {
                throw (e);
                return 0;
            }
        }

        public virtual Task SaveAsync()
        {
            try
            {
                return Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw (e);
            }
            return Task.FromResult(0);
        }



        protected virtual IQueryable<TEntity> GetQueryable<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,

            int? skip = null,
            int? take = null)
            where TEntity : class
        {
            includeProperties = includeProperties ?? string.Empty;
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(
                new char[] { ',' },
                StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }


        public virtual IQueryable<TResult> GetSet<TResult, TKey, TEntity>(
            Expression<Func<TEntity, TResult>> firstSelector,
            Expression<Func<TResult, TKey>> orderBy,
            Expression<Func<TEntity, bool>> filter = null,
            int? skip = null,
            int? take = null)
            where TEntity : class
        {
            var predicates = new List<Expression<Func<TEntity, bool>>> { filter };
            var entities = GetQueryable(filter);
            if (skip != null && take != null)
            {
                return predicates
                    .Aggregate(entities, (current, predicate) => current.Where(predicate))
                    .Select(firstSelector)
                    .OrderBy(orderBy)
                    .Skip(skip.Value)
                    .Take(take.Value);

            }
            else
            {
                return predicates
                    .Aggregate(entities, (current, predicate) => current.Where(predicate))
                    .Select(firstSelector)
                    .OrderBy(orderBy);

            }
        }

        public virtual IQueryable<TReturn> GetGroupedSet<TResult, TKey, TGroup, TReturn, TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TResult>> firstSelector, Expression<Func<TResult, TKey>> orderSelector, Func<TResult, TGroup> groupSelector, Func<IGrouping<TGroup, TResult>, TReturn> selector, int? skip, int? take) where TEntity : class
        {
            var predicates = new List<Expression<Func<TEntity, bool>>> { filter };
            var entities = GetQueryable(filter);

            if (skip != null && take != null)
            {
                return predicates
                    .Aggregate(entities, (current, predicate) => current.Where(predicate))
                    .Select(firstSelector)
                    .OrderBy(orderSelector)
                    .GroupBy(groupSelector)
                    .Skip(skip.Value)
                    .Take(take.Value)
                    .Select(selector).AsQueryable();
            }
            else
            {
                return predicates
                    .Aggregate(entities, (current, predicate) => current.Where(predicate))
                    .Select(firstSelector)
                    .OrderBy(orderSelector)
                    .GroupBy(groupSelector)?
                    .Select(selector)?.AsQueryable();
            }
        }


        public virtual IList<TReturn> GetGrouped<TResult, TKey, TGroup, TReturn, TEntity>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TResult>> firstSelector, Expression<Func<TResult, TKey>> orderSelector, Func<TResult, TGroup> groupSelector, Func<IGrouping<TGroup, TResult>, TReturn> selector, int? skip, int? take) where TEntity : class
        {
            var predicates = new List<Expression<Func<TEntity, bool>>> { filter };
            var entities = GetQueryable(filter);

            if (skip != null && take != null)
            {
                return predicates
                    .Aggregate(entities, (current, predicate) => current.Where(predicate))
                    .Select(firstSelector)
                    .OrderBy(orderSelector)
                    .GroupBy(groupSelector)
                    .Skip(skip.Value)
                    .Take(take.Value)
                    .Select(selector)
                    .ToList();
            }
            else
            {
                return predicates
                    .Aggregate(entities, (current, predicate) => current.Where(predicate))
                    .Select(firstSelector)
                    .OrderBy(orderSelector)
                    .GroupBy(groupSelector)
                    .Select(selector)
                    .ToList();
            }
        }






        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Context?.Dispose();
        }


        public static IQueryable<EntityWithCount<TEntity>> GetWithTotal<TEntity>(IQueryable<TEntity> entities, int page, int pageSize) where TEntity : class
        {
            return entities
                .Select(e => new EntityWithCount<TEntity> { Entity = e, Count = entities.Count() })
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public DbContext GetContext()
        {
            return Context;
        }
    }
    public class EntityWithCount<T> where T : class
    {
        public T Entity { get; set; }
        public int Count { get; set; }
    }
}

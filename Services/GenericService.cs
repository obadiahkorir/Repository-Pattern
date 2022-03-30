using RepositoryPattern.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RepositoryPattern.Services
{
    public class GenericService : IGenericService
    {
        protected readonly IGenericRepository GenericRepository;
        private readonly IDBService _dbService;
        public GenericService(IGenericRepository iRepository, IDBService dbService)
        {
            GenericRepository = iRepository;
            _dbService = dbService;
        }

        public void AddRange<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            GenericRepository.AddRange(entities);
            GenericRepository.Save();
        }

        public int Add<TEntity>(TEntity entity)
            where TEntity : class
        {
            GenericRepository.Add(entity);
            return GenericRepository.Save();
        }

        public void Update<TEntity>(TEntity entity)
            where TEntity : class
        {
            var dbEntry = GenericRepository.GetContext().Entry(entity);


            var id = "";
            try
            {
                id = dbEntry.Property("Id").CurrentValue.ToString();
            }
            catch (Exception ex)
            {

            }

            string changes = "";
            foreach (var property in dbEntry.OriginalValues.Properties)
            {
                try
                {
                    var originalValue = dbEntry.GetDatabaseValues().GetValue<object>(property.Name);
                    var currentValue = dbEntry.Property(property.Name).CurrentValue;

                    if (!Equals(originalValue, currentValue))
                    {
                        changes += property.Name + ": " + originalValue + " -> " + currentValue + "<br />";
                    }
                }
                catch (Exception ex)
                {
                }
            }
            GenericRepository.Save();
        }

        public void Delete<TEntity>(object id)
            where TEntity : class
        {
            GenericRepository.Delete<TEntity>(id);
            GenericRepository.Save();
        }

        public void Delete<TEntity>(TEntity entity)
            where TEntity : class
        {
            GenericRepository.Delete(entity);
            GenericRepository.Save();
        }

        public void DeleteRange<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class
        {
            foreach (var entity in entities)
            {
                GenericRepository.Delete(entity);
            }

            GenericRepository.Save();
        }

        public IEnumerable<TEntity> Get<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class
        {
            return GenericRepository.Get(filter, orderBy, includeProperties, skip, take);
        }

        public IEnumerable<TEntity> GetAll<TEntity>(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class
        {
            return GenericRepository.GetAll(orderBy, includeProperties, skip, take);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class
        {
            return await GenericRepository.GetAllAsync(orderBy, includeProperties, skip, take);
        }

        public async Task<IEnumerable<TEntity>> GetAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = null,
            int? skip = null,
            int? take = null)
            where TEntity : class
        {
            return await GenericRepository.GetAsync(filter, orderBy, includeProperties, skip, take);
        }

        public TEntity GetById<TEntity>(object id)
            where TEntity : class
        {
            return GenericRepository.GetById<TEntity>(id);
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(object id)
            where TEntity : class
        {
            return await GenericRepository.GetByIdAsync<TEntity>(id);
        }


        public void Save()
        {
            GenericRepository.Save();
        }

        public async Task SaveAsync()
        {
            await GenericRepository.SaveAsync();
        }
        public void Dispose()
        {
            GenericRepository.Dispose();
        }
    }
}
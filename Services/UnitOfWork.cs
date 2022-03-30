using RepositoryPattern.Data;
using RepositoryPattern.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryPattern.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private Dictionary<Type, object> _repositories;
        private readonly ApplicationDbContext _context;
        private readonly IDBService _dbService;

        public UnitOfWork(ApplicationDbContext context, IDBService dbService)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
            _dbService = dbService;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            // Checks if the Dictionary Key contains the Model class
            if (_repositories.Keys.Contains(typeof(TEntity)))
            {
                // Return the repository for that Model class
                return _repositories[typeof(TEntity)] as IRepository<TEntity>;
            }
            // If the repository for that Model class doesn't exist, create it
            var repository = new Repository<TEntity>(_context, _dbService);

            // Add it to the dictionary
            _repositories.Add(typeof(TEntity), repository);

            return repository;
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
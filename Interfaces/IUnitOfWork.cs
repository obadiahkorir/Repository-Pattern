using RepositoryPattern.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryPattern.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int Save();
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}
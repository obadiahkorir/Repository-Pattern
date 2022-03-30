using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryPattern.Interfaces
{
    public interface IDBService
    {
        string GeneratePassword(int maxSize);

    }
}
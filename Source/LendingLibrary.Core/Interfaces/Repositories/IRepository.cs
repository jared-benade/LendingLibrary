using System.Collections.Generic;

namespace LendingLibrary.Core.Interfaces.Repositories
{
    public interface IRepository<T> 
    {
        void Save(T entity);
        List<T> GetAll();
        T GetById(int id);
        void DeleteById(string id);
    }
}
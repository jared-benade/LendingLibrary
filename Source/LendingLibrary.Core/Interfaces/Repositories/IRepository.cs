using System.Collections.Generic;

namespace LendingLibrary.Core.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        void Save(T entry);
        List<T> GetAll();
        T GetById(string id);
        void DeleteById(string id);
    }
}
using System.Collections.Generic;
using LendingLibrary.Core.Domain;

namespace LendingLibrary.Core.Interfaces.Repositories
{
    public interface IPersonRepository : IRepository<Person>
    {
        List<Person> GetAllActivePeople();
    }
}
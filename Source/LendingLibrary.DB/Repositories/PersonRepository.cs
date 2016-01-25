using System;
using System.Collections.Generic;
using System.Linq;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;

namespace LendingLibrary.DB.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private ILendingLibraryDbContext dbContext;

        public PersonRepository(ILendingLibraryDbContext dbContext) 
        {
            if (dbContext == null) throw new ArgumentNullException("dbContext");
            this.dbContext = dbContext;
        }

        public void Save(Person entry)
        {
            throw new NotImplementedException();
        }

        public List<Person> GetAll()
        {
            return dbContext.People.ToList();
        }

        public Person GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
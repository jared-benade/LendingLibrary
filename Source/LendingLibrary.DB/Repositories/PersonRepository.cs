using System;
using System.Collections.Generic;
using System.Linq;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;

namespace LendingLibrary.DB.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ILendingLibraryDbContext _dbContext;

        public PersonRepository(ILendingLibraryDbContext dbContext) 
        {
            if (dbContext == null) throw new ArgumentNullException("dbContext");
            this._dbContext = dbContext;
        }

        public void Save(Person entity)
        {
            _dbContext.Upsert(entity);
            _dbContext.SaveChanges();
        }

        public List<Person> GetAll()
        {
            return _dbContext.People.ToList();
        }

        public Person GetById(int id)
        {
            var person = _dbContext.People.AsQueryable().FirstOrDefault(x => x.Id == id);
            return person;
        }

        public void DeleteById(int id)
        {
            var person = GetById(id);
            if (person == null) return;
            _dbContext.People.Remove(person);
            _dbContext.SaveChanges();
        }
    }
}
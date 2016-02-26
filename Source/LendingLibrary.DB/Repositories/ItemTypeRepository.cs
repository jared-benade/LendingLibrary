using System;
using System.Collections.Generic;
using System.Linq;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;

namespace LendingLibrary.DB.Repositories
{
    public class ItemTypeRepository : IItemTypeRepository
    {
        private readonly ILendingLibraryDbContext _dbContext;

        public ItemTypeRepository(ILendingLibraryDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public void Save(ItemType entity)
        {
            throw new System.NotImplementedException();
        }

        public List<ItemType> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public ItemType GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteById(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<ItemType> GetAllActive()
        {
            return _dbContext.ItemTypes.Where(x => x.IsActive).ToList();
        }
    }
}
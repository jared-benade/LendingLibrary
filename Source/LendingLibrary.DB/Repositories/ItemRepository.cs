using System;
using System.Collections.Generic;
using System.Linq;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;

namespace LendingLibrary.DB.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ILendingLibraryDbContext _dbContext;
        public ItemRepository(ILendingLibraryDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public void Save(Item entity)
        {
            _dbContext.Upsert(entity);
            _dbContext.SaveChanges();
        }

        public List<Item> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Item GetById(int id)
        {
            var item = _dbContext.Items.AsQueryable().FirstOrDefault(x => x.Id == id);
            return item;
        }

        public void DeleteById(int id)
        {
            var item = _dbContext.Items.FirstOrDefault(x => x.Id == id);
            if (item == null) return;
            item.IsActive = false;
            Save(item);
        }

        public List<Item> GetAllActive()
        {
            return _dbContext.Items.Where(x => x.IsActive).ToList();
        }
    }
}
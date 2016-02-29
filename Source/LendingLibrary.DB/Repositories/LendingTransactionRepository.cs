using System;
using System.Collections.Generic;
using System.Linq;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces.Repositories;

namespace LendingLibrary.DB.Repositories
{
    public class LendingTransactionRepository : ILendingTransactionRepository
    {
        private readonly ILendingLibraryDbContext _dbContext;

        public LendingTransactionRepository(ILendingLibraryDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException("dbContext");
            _dbContext = dbContext;
        }

        public void Save(LendingTransaction entity)
        {
            _dbContext.Upsert(entity);
            _dbContext.SaveChanges();
        }

        public List<LendingTransaction> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public LendingTransaction GetById(int id)
        {
            var lendingTransaction = _dbContext.LendingTransactions.AsQueryable().FirstOrDefault(x => x.Id == id);
            return lendingTransaction;
        }

        public void DeleteById(int id)
        {
            var lendingTransaction = _dbContext.LendingTransactions.FirstOrDefault(x => x.Id == id);
            if (lendingTransaction == null) return;
            lendingTransaction.IsActive = false;
            Save(lendingTransaction);
        }

        public List<LendingTransaction> GetAllActive()
        {
            return _dbContext.LendingTransactions.Where(x => x.IsActive).ToList();
        }
    }
}
using System.Data.Common;
using System.Data.Entity;
using LendingLibrary.Core.Domain;
using LendingLibrary.Core.Interfaces;

namespace LendingLibrary.DB
{
    public interface ILendingLibraryDbContext
    {
        DbSet<Person> People { get; set; }
        int SaveChanges();
        void Upsert<TEntity>(TEntity entity) where TEntity : class;
    }

    public class LendingLibraryDbContext : DbContext, ILendingLibraryDbContext
    {
        public LendingLibraryDbContext() : this("DefaultConnection")
        {
        }

        public LendingLibraryDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public LendingLibraryDbContext(DbConnection connection) : base(connection, true)
        {
        }

        public void Upsert<TEntity>(TEntity entity) where TEntity : class
        {
            var existingEntity = GetExistingEntity(entity);
            if (existingEntity != null)
            {
                var attachedEntry = Entry(existingEntity);
                attachedEntry.CurrentValues.SetValues(entity);
            }
            else
            {
                var dbset = Set<TEntity>();
                dbset.Add(entity);
            }
        }

        private TEntity GetExistingEntity<TEntity>(TEntity entity) where TEntity : class
        {
            var entityType = (entity as IEntity);
            if (entityType == null)
                return null;

            var dbset = Set<TEntity>();
            return dbset.Find(entityType.Id);
        }

        public DbSet<Person> People { get; set; }
    }
}

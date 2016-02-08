﻿using System.Data.Common;
using System.Data.Entity;
using LendingLibrary.Core.Domain;

namespace LendingLibrary.DB
{
    public interface ILendingLibraryDbContext
    {
        DbSet<Person> People { get; set; }
        int SaveChanges();
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

        public DbSet<Person> People { get; set; }
    }
}

using System;
using FluentMigrator.Runner.Processors.SqlServer;
using PeanutButter.FluentMigrator;

namespace LendingLibrary.DB.Migrations
{
    public class Migrator : DBMigrationsRunner<SqlServer2000ProcessorFactory>
    {
        public Migrator(string connectionString, Action<string> textWriterAction = null)
            : base(typeof(Migrator).Assembly, connectionString, textWriterAction)
        {
        }
    }
}
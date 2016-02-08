using FluentMigrator;
using _Table = LendingLibrary.DB.Migrations.DataConstants.Tables.Person;

namespace LendingLibrary.DB.Migrations.Migrations
{
    [Migration(201601291528)]
    public class _201601291528_DeletePersonTable : Migration
    {
        public override void Up()
        {
            Delete.Table(_Table.NAME);
        }

        public override void Down()
        {
        }
    }
}
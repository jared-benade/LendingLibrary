using FluentMigrator;
using _Table = LendingLibrary.DB.Migrations.DataConstants.Tables.Person;
using _Columns = LendingLibrary.DB.Migrations.DataConstants.Tables.Person.Columns;

namespace LendingLibrary.DB.Migrations.Migrations
{
    [Migration(201601291531)]
    public class _201601291531_CreatePersonTable : Migration
    {
        public override void Up()
        {
            Create.Table(_Table.NAME)
             .WithColumn(_Columns.ID).AsInt32().PrimaryKey()
             .WithColumn(_Columns.FIRSTNAME).AsString().NotNullable()
             .WithColumn(_Columns.LASTNAME).AsString().NotNullable()
             .WithColumn(_Columns.CONTACTNUMBER).AsString()
             .WithColumn(_Columns.EMAIL).AsString();
        }

        public override void Down()
        {
        }
    }
}
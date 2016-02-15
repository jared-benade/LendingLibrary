using FluentMigrator;
using _Table = LendingLibrary.DB.Migrations.DataConstants.Tables.Person;
using _Columns = LendingLibrary.DB.Migrations.DataConstants.Tables.Person.Columns;

namespace LendingLibrary.DB.Migrations.Migrations
{
    [Migration(201507130939)]
    public class _201601221631_CreatePeopleTable : Migration
    {
        public override void Up()
        {
            Create.Table(_Table.NAME)
                .WithColumn(_Columns.ID).AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn(_Columns.FIRSTNAME).AsString().NotNullable()
                .WithColumn(_Columns.LASTNAME).AsString().NotNullable()
                .WithColumn(_Columns.CONTACTNUMBER).AsString().Nullable()
                .WithColumn(_Columns.EMAIL).AsString().Nullable();
        }

        public override void Down()
        {
        }
    }
}
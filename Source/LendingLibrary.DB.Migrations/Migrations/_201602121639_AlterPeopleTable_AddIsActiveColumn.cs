using FluentMigrator;
using _Table = LendingLibrary.DB.Migrations.DataConstants.Tables.Person;
using _Columns = LendingLibrary.DB.Migrations.DataConstants.Tables.Person.Columns;

namespace LendingLibrary.DB.Migrations.Migrations
{
    [Migration(201602121639)]
    public class _201602121639_AlterPeopleTable_AddIsActiveColumn : Migration
    {
        public override void Up()
        {
            Alter.Table(_Table.NAME)
                .AddColumn(_Columns.ISACTIVE).AsBoolean().NotNullable().WithDefaultValue(true);
        }

        public override void Down()
        {
        }
    }
}
using FluentMigrator;
using _Table = LendingLibrary.DB.Migrations.DataConstants.Tables.ItemType;
using _Columns = LendingLibrary.DB.Migrations.DataConstants.Tables.ItemType.Columns;

namespace LendingLibrary.DB.Migrations.Migrations
{
    [Migration(201602151550)]
    public class _201602151550_CreateItemTypeTable : Migration
    {
        public override void Up()
        {
            Create.Table(_Table.NAME)
                .WithColumn(_Columns.ID).AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn(_Columns.DESCRIPTION).AsString().Nullable()
                .WithColumn(_Columns.ISACTIVE).AsBoolean().NotNullable().WithDefaultValue(true);
        }

        public override void Down()
        {
        }
    }
}
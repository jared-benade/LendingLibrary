using FluentMigrator;
using _Table = LendingLibrary.DB.Migrations.DataConstants.Tables.Item;
using _ForeignTable = LendingLibrary.DB.Migrations.DataConstants.Tables.ItemType;
using _Columns = LendingLibrary.DB.Migrations.DataConstants.Tables.Item.Columns;

namespace LendingLibrary.DB.Migrations.Migrations
{
    [Migration(201602151553)]
    public class _201602151553_CreateItemTable : Migration
    {
        public override void Up()
        {
            Create.Table(_Table.NAME)
                .WithColumn(_Columns.ID).AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn(_Columns.TITLE).AsString().Nullable()
                .WithColumn(_Columns.AUTHOR).AsString().Nullable()
                .WithColumn(_Columns.EDITION).AsString().Nullable()
                .WithColumn(_Columns.YEAR).AsString().Nullable()
                .WithColumn(_Columns.ARTIST).AsString().Nullable()
                .WithColumn(_Columns.AVAILABLE).AsBoolean().NotNullable().WithDefaultValue(true)
                .WithColumn(_Columns.ITEMTYPEID).AsInt32().NotNullable().ForeignKey("FK_Item_ItemType", _ForeignTable.NAME, _ForeignTable.Columns.ID)
                .WithColumn(_Columns.ISACTIVE).AsBoolean().NotNullable().WithDefaultValue(true);
        }

        public override void Down()
        {
        }
    }
}
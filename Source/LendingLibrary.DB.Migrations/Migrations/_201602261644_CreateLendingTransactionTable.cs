using FluentMigrator;
using _Table = LendingLibrary.DB.Migrations.DataConstants.Tables.LendingTransaction;
using _Columns = LendingLibrary.DB.Migrations.DataConstants.Tables.LendingTransaction.Columns;
using _ItemForeignTable = LendingLibrary.DB.Migrations.DataConstants.Tables.Item;
using _PersonForeignTable = LendingLibrary.DB.Migrations.DataConstants.Tables.Person;

namespace LendingLibrary.DB.Migrations.Migrations
{
    [Migration(201602261644)]
    public class _201602261644_CreateLendingTransactionTable : Migration
    {
        public override void Up()
        {
            Create.Table(_Table.NAME)
                .WithColumn(_Columns.ID).AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn(_Columns.DATEBORROWED).AsDateTime().NotNullable()
                .WithColumn(_Columns.DATERETURNED).AsDateTime().Nullable()
                .WithColumn(_Columns.PERSONID).AsInt32().NotNullable().ForeignKey("FK_LendingTransaction_Person", _PersonForeignTable.NAME, _PersonForeignTable.Columns.ID)
                .WithColumn(_Columns.ITEMID).AsInt32().NotNullable().ForeignKey("FK_LendingTransaction_Item", _ItemForeignTable.NAME, _ItemForeignTable.Columns.ID)
                .WithColumn(_Columns.ISACTIVE).AsBoolean().NotNullable().WithDefaultValue(true);
        }

        public override void Down()
        {
        }
    }
}
using FluentMigrator;

namespace LendingLibrary.DB.Migrations.Migrations
{
    [Migration(201602261504)]
    public class _201602261504_ItemTypeInitialDataImport : Migration
    {
        public override void Up()
        {
            Execute.EmbeddedScript("ItemTypeDataImport.sql");
        }

        public override void Down()
        {
        }
    }
}
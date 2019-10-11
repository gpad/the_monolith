using FluentMigrator;

namespace TheMonolith.Migrations
{
    [Migration(201910111804003)]
    public class AddProductsTable : Migration
    {
        public override void Down()
        {
            Delete.Table("products");
        }

        public override void Up()
        {
            Create.Table("products")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("name").AsString(255)
                .WithColumn("description").AsString(1024)
                .WithColumn("price").AsCurrency()
                .WithColumn("qty").AsInt32()
                .WithColumn("sellable").AsBoolean().NotNullable()
                ;
        }
    }
}

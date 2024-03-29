using FluentMigrator;

namespace TheMonolith.Migrations
{
    [Migration(201910111804006)]
    public class AddCartItemsTable : Migration
    {
        public override void Down()
        {
            Delete.Table("cart_items");
        }

        public override void Up()
        {
            Create.Table("cart_items")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("product_id").AsString(255)
                .WithColumn("name").AsString(1024)
                .WithColumn("price").AsCurrency()
                .WithColumn("qty").AsInt16()
                .WithColumn("cart_id").AsGuid().ForeignKey("carts", "id").NotNullable().Indexed();
                ;
        }
    }
}

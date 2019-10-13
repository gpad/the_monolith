using FluentMigrator;

namespace TheMonolith.Migrations
{
    [Migration(201910111804010)]
    public class AddInvoicesTable : Migration
    {
        public override void Down()
        {
            Delete.Table("invoices");
        }

        public override void Up()
        {
            Create.Table("invoices")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("number").AsString().Unique()
                .WithColumn("first_name").AsString(255)
                .WithColumn("last_name").AsString(255)
                .WithColumn("address").AsString()
                .WithColumn("total").AsCurrency()
                .WithColumn("customer_id").AsGuid().ForeignKey("customers", "id")
                .WithColumn("cart_id").AsGuid().ForeignKey("carts", "id")
                .WithColumn("status").AsString().NotNullable()
                ;
        }
    }
}

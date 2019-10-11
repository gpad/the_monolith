using FluentMigrator;

namespace TheMonolith.Migrations
{
    [Migration(201910111804003)]
    public class AddInvoiceItemsTable : Migration
    {
        public override void Down()
        {
            Delete.Table("invoice_items");
        }

        public override void Up()
        {
            Create.Table("invoice_items")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("product_id").AsString(255)
                .WithColumn("descriptioon").AsString(1024)
                .WithColumn("price").AsCurrency()
                .WithColumn("qty").AsInt16()
                .WithColumn("invoice_id").AsGuid().ForeignKey("invoices", "id").NotNullable().Indexed();
                ;
        }
    }
}

using FluentMigrator;

namespace TheMonolith.Migrations
{
    [Migration(201910111804003)]
    public class AddBillingAddressesTable : Migration
    {
        public override void Down()
        {
            Delete.Table("billing_addresses");
        }

        public override void Up()
        {
            Create.Table("billing_addresses")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("street").AsString(255)
                .WithColumn("city").AsString(1024)
                .WithColumn("country").AsDecimal()
                .WithColumn("customer_id").AsGuid().ForeignKey("customers", "Id").NotNullable();
                ;
        }
    }
}

using FluentMigrator;

namespace TheMonolith.Migrations
{
    [Migration(201910111804003)]
    public class AddShipmentAddressesTable : Migration
    {
        public override void Down()
        {
            Delete.Table("shipment_addresses");
        }

        public override void Up()
        {
            Create.Table("shipment_addresses")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("street").AsString(255)
                .WithColumn("city").AsString(1024)
                .WithColumn("country").AsString()
                .WithColumn("customer_id").AsGuid().ForeignKey("Customers", "Id").NotNullable();
                ;
        }
    }
}

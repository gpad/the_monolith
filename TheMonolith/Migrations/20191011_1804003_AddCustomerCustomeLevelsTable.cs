using FluentMigrator;

namespace TheMonolith.Migrations
{
    [Migration(201910111804003)]
    public class AddCustomerCustomerLevelsTable : Migration
    {
        public override void Down()
        {
            Delete.Table("customer_customer_levels");
        }

        public override void Up()
        {
            Create.Table("Customer_customer_levels")
                .WithColumn("customer_id").AsGuid().ForeignKey("customers", "id").Unique()
                .WithColumn("customer_level_id").AsGuid().ForeignKey("customer_levels", "id")
            ;
        }
    }
}

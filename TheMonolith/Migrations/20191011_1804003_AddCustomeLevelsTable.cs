using FluentMigrator;

namespace TheMonolith.Migrations
{
    [Migration(201910111804003)]
    public class AddCustomerLevelsTable : Migration
    {
        public override void Down()
        {
            Delete.Table("customer_levels");
        }

        public override void Up()
        {
            Create.Table("customer_levels")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("name").AsString(255).NotNullable().Indexed().Unique()
                .WithColumn("description").AsString();
        }
    }
}

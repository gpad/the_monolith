using FluentMigrator;

namespace TheMonolith.Migrations
{
    [Migration(201910111804003)]
    public class AddCustomersTable : Migration
    {
        public override void Down()
        {
            Delete.Table("Customer");
        }

        public override void Up()
        {
            Create.Table("Customer")
                .WithColumn("Id").AsGuid().PrimaryKey()
                .WithColumn("FirstName").AsString(255)
                .WithColumn("LastName").AsString(255)
                .WithColumn("Age").AsInt16()
                ;
        }
    }
}

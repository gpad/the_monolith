using FluentMigrator;

namespace TheMonolith.Migrations
{
    [Migration(201910111804003)]
    public class AddCustomersTable : Migration
    {
        public override void Down()
        {
            Delete.Table("customers");
        }

        public override void Up()
        {
            Create.Table("customers")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("first_name").AsString(255)
                .WithColumn("last_name").AsString(255)
                .WithColumn("age").AsInt16()
                .WithColumn("genders").AsString()
                .WithColumn("email").AsString()
                ;
        }
    }
}

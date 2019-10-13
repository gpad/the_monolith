using FluentMigrator;

namespace TheMonolith.Migrations
{
    [Migration(201910111804001)]
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
                .WithColumn("first_name").AsString(255).NotNullable()
                .WithColumn("last_name").AsString(255).NotNullable()
                .WithColumn("age").AsInt16().Nullable()
                .WithColumn("gender").AsString().Nullable()
                .WithColumn("email").AsString().NotNullable().Indexed()
                ;
        }
    }
}

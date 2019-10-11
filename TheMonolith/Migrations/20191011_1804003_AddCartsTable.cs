using FluentMigrator;

namespace TheMonolith.Migrations
{
    [Migration(201910111804005)]
    public class AddCartsTable : Migration
    {
        public override void Down()
        {
            Delete.Table("carts");
        }

        public override void Up()
        {
            Create.Table("carts")
                .WithColumn("id").AsGuid().PrimaryKey()
                .WithColumn("customer_id").AsGuid().ForeignKey().Nullable()
                .WithColumn("status").AsString().Indexed();
                ;
        }
    }
}

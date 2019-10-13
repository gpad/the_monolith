using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace TheMonolith.ECommerce
{
    public class Warehouse : IWarehouse
    {
        public ILogger logger;
        public Warehouse(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task DismissAsync(Product product)
        {
            logger.LogDebug($"Dismiss product {product.Id}");
            using (var connection = await GetOpenConnection())
            {
                await connection.ExecuteAsync(@"
                update products
                    SET
                        sellable = false
                    where
                        id = @Id
                ", new
                { Id = product.Id });
            }
        }

        public async Task AddNewProductAsync(Product product, int qty)
        {
            logger.LogDebug($"Add new product {product.Id} - {qty}");
            using (var connection = await GetOpenConnection())
            {
                await connection.ExecuteAsync(@"
                insert
                    into products
                        (id, name, description, price, qty, sellable)
                    values
                        (@Id, @Name, @Description, @Price, @Qty, true)
                ", new
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price.Value,
                    Qty = qty
                });
            }
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            using (var connection = await GetOpenConnection())
            {
                var results = await connection.QueryAsync(@"Select * from products where sellable = true and qty > 0");
                logger.LogDebug($"Get Active products {results.Count()}");
                return results.Select(row =>
                {
                    return new Product(Guid.Parse(row.id), row.name, row.description, new Money((decimal)row.price, Currency.EUR));
                });
            }
        }

        public async Task RefillAsync(Product product, int qtyToAdd)
        {
            logger.LogDebug($"Refill product {product.Id} - {qtyToAdd}");
            using (var connection = await GetOpenConnection())
            {
                await connection.ExecuteAsync(@"
                update products
                    SET
                        qty = qty + @Qty
                    where
                        id = @Id
                ", new
                { Id = product.Id, Qty = qtyToAdd });
            }
        }

        private static SqliteConnection GetConnection()
        {
            return new SqliteConnection("Data Source=TheMonolith.db");
        }

        private async Task<SqliteConnection> GetOpenConnection()
        {
            var connection = GetConnection();
            await connection.OpenAsync();
            return connection;
        }
    }
}

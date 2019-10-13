using System;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace TheMonolith.ECommerce
{
    public class Shop : IShop
    {
        private readonly ILogger logger;
        public Shop(ILogger logger)
        {
            this.logger = logger;
        }

        public Task CheckoutAsync(Customer customer)
        {
            throw new System.NotImplementedException();
        }

        public Task PayAsync(Customer customer)
        {
            throw new System.NotImplementedException();
        }

        public async Task PutProductInCartAsync(Customer customer, Product product)
        {
            logger.LogDebug($"Add Items in cart of cutmer_id {customer.Id}");
            using (var connection = await GetOpenConnection())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var cart_id = await GetActiveCartOf(customer, transaction);
                    await connection.ExecuteAsync(
                        @"
                        insert into cart_items
                            (id, product_id, name, price, qty, cart_id)
                        values
                            (@id, @product_id, @name, @price, @qty, @cart_id)
                        ",new {
                            id = Guid.NewGuid(),
                            product_id = product.Id,
                            name = product.Name,
                            price = product.Price.Value,
                            qty = 1,
                            cart_id = cart_id
                        }, transaction);
                    await transaction.CommitAsync();
                }

            }
        }

        private async Task<Guid> GetActiveCartOf(Customer customer, DbTransaction transaction)
        {
            logger.LogDebug($"Get active cart of cutmer_id {customer.Id}");
            var str = await transaction.Connection.QuerySingleAsync<string>(
                @"Select id from carts where customer_id = @customer_id and status = 'active'",
                new { customer_id = customer.Id });
            return Guid.Parse(str);
        }

        public async Task ResetCartAsync(Customer customer)
        {
            logger.LogDebug($"ResetCartAsync cutmer_id {customer.Id}");
            using (var connection = await GetOpenConnection())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    await connection.ExecuteAsync(@"
                        update carts
                            SET
                                status = 'dismissed'
                            where
                                customer_id = @Id and
                                status = 'active'
                        ", new
                    { Id = customer.Id }, transaction);
                    await connection.ExecuteAsync(@"
                        insert into carts
                            (id, status, customer_id)
                        values
                            (@id, 'active', @customer_id)
                        ", new {
                            id = Guid.NewGuid(),
                            customer_id = customer.Id
                        }, transaction);

                    await transaction.CommitAsync();
                }
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

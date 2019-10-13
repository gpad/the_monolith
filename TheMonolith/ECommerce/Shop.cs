using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace TheMonolith.ECommerce
{
    class Cart
    {
        public Money Total { get; }
        public Guid Id { get; }

        public Cart(Guid id, Money total)
        {
            this.Id = id;
            this.Total = total;
        }
    }

    public class Shop : IShop
    {
        private readonly ILogger logger;
        private readonly Random random = new Random();

        public Shop(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<Guid> CheckoutAsync(Customer customer)
        {
            logger.LogDebug($"Add Items in cart of cutmer_id {customer.Id}");
            using (var connection = await GetOpenConnection())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var invoice_id = Guid.NewGuid();
                    var cart = await GetActiveCartAsync(customer, transaction);
                    await connection.ExecuteAsync(
                        @"
                        insert into invoices
                            (id, number, first_name, last_name, address, total, customer_id, cart_id, status)
                        values
                            (@id, @number, @first_name, @last_name, @address, @total, @customer_id, @cart_id, @status)
                        ", new
                        {
                            id = invoice_id,
                            number = await GetNextInvoiceNumberAsync(transaction),
                            first_name = customer.FirstName,
                            last_name = customer.LastName,
                            address = await GetShipmentAddressOfAsync(customer, transaction),
                            total = cart.Total.Value,
                            customer_id = customer.Id,
                            cart_id = cart.Id,
                            status = "payable"
                        }, transaction);
                    await transaction.CommitAsync();
                    return invoice_id;

                }
            }
        }

        public async Task PayAsync(Customer customer, Guid invoice_id)
        {
            logger.LogDebug($"Pay invoice {invoice_id} {customer.Id}");
            await Task.Delay(random.Next(2000)); // Simulate payment
            using (var connection = await GetOpenConnection())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var affectedRows = await connection.ExecuteAsync(
                        @"
                        update invoices
                            SET status = 'paid'
                        where
                            id = @id and customer_id = @customer_id
                        ", new
                        {
                            id = invoice_id,
                            customer_id = customer.Id,
                        }, transaction);

                    if (affectedRows != 1)
                        throw new ArgumentException($"Unable to update invoice {invoice_id} for customer {customer.Id}");
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task PutProductInCartAsync(Customer customer, Product product)
        {
            logger.LogDebug($"Add Items in cart of cutmer_id {customer.Id}");
            using (var connection = await GetOpenConnection())
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    var cart = await GetActiveCartAsync(customer, transaction);
                    await connection.ExecuteAsync(
                        @"
                        insert into cart_items
                            (id, product_id, name, price, qty, cart_id)
                        values
                            (@id, @product_id, @name, @price, @qty, @cart_id)
                        ", new
                        {
                            id = Guid.NewGuid(),
                            product_id = product.Id,
                            name = product.Name,
                            price = product.Price.Value,
                            qty = 1,
                            cart_id = cart.Id
                        }, transaction);
                    await transaction.CommitAsync();
                }

            }
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
                        ", new
                    {
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
        private async Task<string> GetShipmentAddressOfAsync(Customer customer, DbTransaction transaction)
        {
            var row = await transaction.Connection.QueryFirstAsync(
                @"select * from shipment_addresses where customer_id = @cusotmer_id",
                new { cusotmer_id = @customer.Id });

            return $"{row.street} {row.city} {row.country}";
        }

        private Task<string> GetNextInvoiceNumberAsync(DbTransaction transaction)
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }

        private async Task<Cart> GetActiveCartAsync(Customer customer, DbTransaction transaction)
        {
            logger.LogDebug($"Get active cart of cutmer_id {customer.Id}");
            var strId = await transaction.Connection.QuerySingleAsync<string>(
               @"Select id from carts where customer_id = @customer_id and status = 'active'",
               new { customer_id = customer.Id });

            var items = await transaction.Connection.QueryAsync(@"Select * from cart_items where cart_id = @cart_id", new { cart_id = strId });

            var total = items.Aggregate(0M, (acc, row) => acc + row.price);
            return new Cart(Guid.Parse(strId), new Money(total, Currency.EUR));
        }

    }
}

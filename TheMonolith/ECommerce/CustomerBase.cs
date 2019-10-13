using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace TheMonolith.ECommerce
{
    public class CustomerBase : ICustomerBase
    {
        private readonly ILogger logger;

        public CustomerBase(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<IEnumerable<Customer>> ActiveCustomerAsync()
        {
            using (var connection = await GetOpenConnection())
            {
                var results = await connection.QueryAsync(@"Select * from customers");
                logger.LogDebug($"Get Active customers {results.Count()}");
                return results.Select(row =>
                {
                    return new Customer(Guid.Parse(row.id),
                        row.first_name,
                        row.last_name,
                        (int)row.age,
                        row.email);

                });
            }

        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            logger.LogDebug($"Add new Customer {customer.Id}");
            using (var connection = await GetOpenConnection())
            {
                await connection.ExecuteAsync(@"
                insert
                    into customers
                        (id, first_name, last_name, age, email)
                    values
                        (@Id, @first_name, @last_name, @age, @email)
                ", new
                {
                    Id = customer.Id,
                    first_name = customer.FirstName,
                    last_name = customer.LastName,
                    age = customer.Age,
                    email = customer.Email
                });
            }
            return customer;
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

        public async Task<Guid> CreateShipmentAddressAsync(Customer customer, string v1, string v2, string v3)
        {
            logger.LogDebug($"Add ShipmentAddress for customer {customer.Id}");
            var address_id = Guid.NewGuid();
            using (var connection = await GetOpenConnection())
            {
                await connection.ExecuteAsync(@"
                insert
                    into shipment_addresses
                        (id, street, city, country, customer_id)
                    values
                        (@id, @street, @city, @country, @customer_id)
                ", new{
                    id = address_id,
                    street = v1,
                    city = v2,
                    country = v3,
                    customer_id = customer.Id
                });
            }
            return address_id;
        }
    }
}

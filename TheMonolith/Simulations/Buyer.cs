using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TheMonolith.ECommerce;

namespace TheMonolith.Simulations
{
    public class Buyer : ISimulation
    {
        private static Random Random = new Random();
        private readonly ICustomerBase CustomerBase;
        private readonly IWarehouse Warehouse;
        private readonly IShop Shop;
        private readonly ILogger logger;

        public Buyer(ILogger logger, ICustomerBase customerBase, IWarehouse warehouse, IShop shop)
        {
            this.logger = logger;
            CustomerBase = customerBase;
            Warehouse = warehouse;
            Shop = shop;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("--- Start Buyer ---");
            await Task.Delay(WaitForNextStep(), stoppingToken);
            var customer = await ImpersonateCustomer();
            await ResetCart(customer);
            int count = 1 + Random.Next(10);
            for (int i = 0; i < count && !stoppingToken.IsCancellationRequested; i++)
            {
                var product = await ChoseProduct();
                if (product == null) return;
                await PutProductInCart(customer, product);
                await Task.Delay(WaitForNextStep(), stoppingToken);
            }
            if (FlipCoin() || stoppingToken.IsCancellationRequested) return;
            await Task.Delay(WaitForNextStep(), stoppingToken);
            var invoice_id = await Checkout(customer);
            if (FlipCoin() || stoppingToken.IsCancellationRequested) return;
            await Task.Delay(WaitForNextStep(), stoppingToken);
            await Pay(customer, invoice_id);
        }

        private TimeSpan WaitForNextStep()
        {
            return TimeSpan.FromMilliseconds(Random.Next(1000));
        }

        private async Task ResetCart(Customer customer)
        {
            await Shop.ResetCartAsync(customer);
        }

        private async Task Pay(Customer customer, Guid invoice_id)
        {
            await Shop.PayAsync(customer, invoice_id);
        }

        private async Task<Guid> Checkout(Customer customer)
        {
            return await Shop.CheckoutAsync(customer);
        }

        private async Task PutProductInCart(Customer customer, Product product)
        {
            await Shop.PutProductInCartAsync(customer, product);
        }

        private async Task<Product> ChoseProduct()
        {
            var products = await Warehouse.GetActiveProductsAsync();
            return products.OrderBy(c => System.Guid.NewGuid()).FirstOrDefault();
        }

        private async Task<Customer> ImpersonateCustomer()
        {
            var customers = await CustomerBase.ActiveCustomerAsync();
            if (customers.Count() == 0 || FlipCoin())
            {
                var customer = await CustomerBase.CreateCustomerAsync(CreateNewCustomer());
                await CustomerBase.CreateShipmentAddressAsync(customer, "Via Giulio Cesare 13", "Rome", "Italy");
                return customer;
            }
            return customers.OrderBy(c => System.Guid.NewGuid()).First();
        }

        private Customer CreateNewCustomer()
        {
            var id = Guid.NewGuid();
            return new Customer(id,
                $"FirstName{id}",
                $"LastName{id}",
                18 + Random.Next(82),
                $"e{id}@test.com");
        }

        private bool FlipCoin()
        {
            return (Random.Next() % 2) == 0;
        }
    }
}

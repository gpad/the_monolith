using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TheMonolith.ECommerce;

namespace TheMonolith.Simulations
{
    public class Buyer : ISimulation
    {
        private static Random Random = new Random();
        private readonly ICustomerBase CustomerBase;
        private readonly IWarehouse Warehouse;
        private readonly IShop Shop;

        public Buyer(ICustomerBase customerBase, IWarehouse warehouse, IShop shop)
        {
            CustomerBase = customerBase;
            Warehouse = warehouse;
            Shop = shop;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {

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
            await Checkout(customer);
            if (FlipCoin() || stoppingToken.IsCancellationRequested) return;
            await Task.Delay(WaitForNextStep(), stoppingToken);
            await Pay(customer);
        }

        private TimeSpan WaitForNextStep()
        {
            return TimeSpan.FromMilliseconds(Random.Next(1000));
        }

        private async Task ResetCart(Customer customer)
        {
            await Shop.ResetCartAsync(customer);
        }

        private async Task Pay(Customer customer)
        {
            await Shop.PayAsync(customer);
        }

        private async Task Checkout(Customer customer)
        {
            await Shop.CheckoutAsync(customer);
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
                return await CustomerBase.CreateCustomerAsync(CreateNewCustomer());
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

using System;
using System.Linq;
using System.Threading.Tasks;
using TheMonolith.Shop;

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

        public async Task Start()
        {
            await Task.Delay(WaitForNextStep());
            var customer = await ImpersonateCustomer();
            await ResetCart(customer);
            int count = 1 + Random.Next(10);
            while (count > 0)
            {
                var product = await ChoseProduct();
                await PutProductInCart(customer, product);
                await Task.Delay(WaitForNextStep());
            }
            if (FlipCoin()) return;
            await Task.Delay(WaitForNextStep());
            await Checkout(customer);
            if (FlipCoin()) return;
            await Task.Delay(WaitForNextStep());
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
            return products.OrderBy(c => System.Guid.NewGuid()).First();
        }

        private async Task<Customer> ImpersonateCustomer()
        {
            var customers = await CustomerBase.ActiveCustomerAsync();
            return customers.OrderBy(c => System.Guid.NewGuid()).First();
        }

        private bool FlipCoin()
        {
            return (Random.Next() % 2) == 0;
        }
    }
}

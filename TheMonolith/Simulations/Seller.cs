using System;
using System.Linq;
using System.Threading.Tasks;
using TheMonolith.ECommerce;

namespace TheMonolith.Simulations
{

    public class Seller : ISimulation
    {
        private static Random Random = new Random();
        private readonly IWarehouse Warehouse;

        public Seller(IWarehouse warehouse)
        {
            Warehouse = warehouse;
        }

        public async Task Start()
        {
            await Task.Delay(WaitForNextStep());
            await ChargeNewProductAsync();
            await Task.Delay(WaitForNextStep());
            await ReFillSomeProductsAsync();
            await Task.Delay(WaitForNextStep());
            await DismissSomeProductsAsync();
        }

        private async Task DismissSomeProductsAsync()
        {
            var products = await Warehouse.GetActiveProductsAsync();
            products.TakeWhile(p => Random.Next() % 4 == 0).Select(p => Warehouse.DismissAsync(p));
        }

        private TimeSpan WaitForNextStep()
        {
            return TimeSpan.FromMilliseconds(Random.Next(1000));
        }

        private async Task ReFillSomeProductsAsync()
        {
            var products = await Warehouse.GetActiveProductsAsync();
            products.TakeWhile(p => Random.Next() % 4 == 0).Select(p => Warehouse.RefillAsync(p, 1 + Random.Next(100)));
        }

        private async Task ChargeNewProductAsync()
        {
            for (int i = 0; i < Random.Next(100); i++)
            {
                var product = CreateNewProduct();
                await Warehouse.FillNewProductAsync(product, Random.Next(100));
            }
        }

        private Product CreateNewProduct()
        {
            var id = Guid.NewGuid();
            return new Product(id,
                $"title - {id}",
                $"description for {id}",
                new Money(1 + Random.Next(100), new Currency("EUR")));
        }
    }
}

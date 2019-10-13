using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TheMonolith.ECommerce;

namespace TheMonolith.Simulations
{
    public class Seller : ISimulation
    {
        private static Random Random = new Random();
        private readonly IWarehouse Warehouse;
        private readonly ILogger logger;

        public Seller(ILogger logger, IWarehouse warehouse)
        {
            this.logger = logger;
            Warehouse = warehouse;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("+++ Start Seller +++");
            await Task.Delay(WaitForNextStep(), stoppingToken);
            await AddNewProductsAsync(stoppingToken);
            await Task.Delay(WaitForNextStep(), stoppingToken);
            await ReFillSomeProductsAsync(stoppingToken);
            await Task.Delay(WaitForNextStep(), stoppingToken);
            await DismissSomeProductsAsync(stoppingToken);
        }

        private async Task DismissSomeProductsAsync(CancellationToken stoppingToken)
        {
            var products = await Warehouse.GetActiveProductsAsync();
            var tasks = products.Shuffle().Take(products.Count() / 4)
                .TapList(l => logger.LogInformation($"Dismiss {l.Count()} products"))
                .Select(p => Warehouse.DismissAsync(p));
            await Task.WhenAll(tasks);
        }

        private async Task ReFillSomeProductsAsync(CancellationToken stoppingToken)
        {
            var products = await Warehouse.GetActiveProductsAsync();
            var tasks = products.Shuffle().Take(products.Count() / 4)
                .TapList(l => logger.LogInformation($"Refill {l.Count()} products"))
                .Select(p => Warehouse.RefillAsync(p, 1 + Random.Next(100)));
            await Task.WhenAll(tasks);
        }

        private async Task AddNewProductsAsync(CancellationToken stoppingToken)
        {
            int v = Random.Next(100);
            logger.LogInformation($"Add {v} new products");
            for (int i = 0; i < v; i++)
            {
                if (stoppingToken.IsCancellationRequested)
                    return;
                var product = CreateNewProduct();
                await Warehouse.AddNewProductAsync(product, Random.Next(100));
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

        private TimeSpan WaitForNextStep()
        {
            return TimeSpan.FromMilliseconds(Random.Next(1000));
        }
    }
}

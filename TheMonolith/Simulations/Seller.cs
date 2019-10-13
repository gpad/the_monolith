using System;
using System.Collections.Generic;
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
            await WaitForNextStep(stoppingToken);
            await AddNewProductsAsync(stoppingToken);
            await WaitForNextStep(stoppingToken);
            await ReFillSomeProductsAsync(stoppingToken);
            await WaitForNextStep(stoppingToken);
            await DismissSomeProductsAsync(stoppingToken);
        }

        private async Task DismissSomeProductsAsync(CancellationToken stoppingToken)
        {
            var products = await Warehouse.GetActiveProductsAsync();
            var tasks = products.Shuffle().Take(HowMany(products))
                .TapList(l => logger.LogInformation($"Dismiss {l.Count()} products"))
                .Select(product => Warehouse.DismissAsync(product));
            await Task.WhenAll(tasks);
        }

        private async Task ReFillSomeProductsAsync(CancellationToken stoppingToken)
        {
            var products = await Warehouse.GetActiveProductsAsync();
            var tasks = products.Shuffle().Take(HowMany(products))
                .TapList(l => logger.LogInformation($"Refill {l.Count()} products"))
                .Select(p => Warehouse.RefillAsync(p, 1 + Random.Next(100)));
            await Task.WhenAll(tasks);
        }

        private static int HowMany<T>(IEnumerable<T> list)
        {
            return (list.Count() / 4) % 30;
        }

        private async Task AddNewProductsAsync(CancellationToken stoppingToken)
        {
            int v = Random.Next(10);
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

        private async Task WaitForNextStep(CancellationToken stoppingToken)
        {
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(Random.Next(1000));
            await Task.Delay(timeSpan, stoppingToken);
        }
    }
}

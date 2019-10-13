using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TheMonolith.ECommerce;

namespace TheMonolith.Simulations
{
    public class Simulations
    {
        private readonly Random Random = new Random();
        private readonly IWarehouse Warehouse;
        private readonly ICustomerBase CustomerBase;
        private readonly IShop Shop;
        public ILogger Logger { get; }

        public Simulations(ILogger logger, IWarehouse warehouse, ICustomerBase customerBase, IShop shop)
        {
            this.Logger = logger;
            Warehouse = warehouse;
            CustomerBase = customerBase;
            Shop = shop;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Logger.LogInformation("*** Start cycle of simulation ***");
                var tasks = Enumerable.Range(1, 10)
                    .Select(n => FlipCoin() ? (ISimulation)new Seller(Logger, Warehouse) : (ISimulation)new Buyer(CustomerBase, Warehouse, Shop))
                    .Select(simulation => simulation.StartAsync(stoppingToken));
                await Task.WhenAll(tasks);
            }
        }

        private bool FlipCoin()
        {
            return (Random.Next() % 2) == 0;
        }
    }
}

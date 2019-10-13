using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TheMonolith.Shop;

namespace TheMonolith.Simulations
{
    public class Simulations
    {
        private readonly Random Random = new Random();
        private readonly IWarehouse Warehouse;
        private readonly ICustomerBase CustomerBase;
        private readonly IShop Shop;

        public Simulations(IWarehouse warehouse, ICustomerBase customerBase, IShop shop)
        {
            Warehouse = warehouse;
            CustomerBase = customerBase;
            Shop = shop;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var tasks = Enumerable.Range(1, 100).Select(n => FlipCoin() ? (ISimulation)new Seller(Warehouse) : (ISimulation)new Buyer(CustomerBase, Warehouse, Shop)).Select(simulation => simulation.Start());
                await Task.WhenAll(tasks);
            }
        }

        private bool FlipCoin()
        {
            return (Random.Next() % 2) == 0;
        }
    }
}

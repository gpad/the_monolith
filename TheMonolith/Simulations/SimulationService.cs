using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TheMonolith.ECommerce;

namespace TheMonolith.Simulations
{
    internal class SimulationService : BackgroundService
    {
        public ILogger<SimulationService> logger;
        public SimulationService(ILogger<SimulationService> logger)
        {
            this.logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Start Simulation");
            var simulations = new Simulations(logger, new Warehouse(logger), new CustomerBase(logger), new Shop(logger));
            return simulations.StartAsync(stoppingToken);
        }
    }
}

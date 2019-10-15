using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TheMonolith.ECommerce;

namespace TheMonolith.Simulations
{
    internal class SimulationService : BackgroundService
    {
        private readonly string connectionString;
        public ILogger<SimulationService> logger;
        public SimulationService(ILogger<SimulationService> logger, IConfiguration config)
        {
            this.logger = logger;
            this.connectionString = config.GetValue<string>("ASBConnectionString");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Start Simulation");
            var simulations = new Simulations(
                logger,
                new Warehouse(logger),
                new CustomerBase(logger),
                new Shop(logger, connectionString));
            return simulations.StartAsync(stoppingToken);
        }
    }
}

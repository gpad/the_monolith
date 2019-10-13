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
        public ILogger<SimulationService> Logger { get; }
        public SimulationService(ILogger<SimulationService> logger)
        {
            this.Logger = logger;

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Start Simulation");
            var simulations = new Simulations(Logger, new Warehouse(), new CustomerBase(), new Shop());
            return simulations.StartAsync(stoppingToken);
        }
    }
}

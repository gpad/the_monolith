using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TheMonolith.Simulations
{
    internal class SimulationService : BackgroundService
    {
        private readonly ServiceProvider ServiceProvider;
        public SimulationService(ServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var simulations = ServiceProvider.GetRequiredService<Simulations>();
            return simulations.StartAsync(stoppingToken);
        }
    }
}

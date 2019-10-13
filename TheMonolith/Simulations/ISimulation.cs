using System.Threading;
using System.Threading.Tasks;

namespace TheMonolith.Simulations
{
    public interface ISimulation
    {
        Task StartAsync(CancellationToken stoppingToken);
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TheMonolith.Simulations;
using TheMonolith.Migrations;
using TheMonolith.Infrastructure;
using System.Threading.Tasks;

namespace TheMonolith
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            Runner.RunDbMigration();
            var infrastructure = await AzureInfrastracture.CreateInfrastructureAsync("test-LB-AS");

            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(config =>
                {
                    config.AddCommandLine(args);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).ConfigureServices(services =>
         {
             services.AddHostedService<SimulationService>();
         });
        }
    }
}

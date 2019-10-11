using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace TheMonolith
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // var serviceProvider = CreateServices();

            // Put the database update into a scope to ensure
            // that all resources will be disposed.
            // using (var scope = serviceProvider.CreateScope())
            // {
            //     UpdateDatabase(scope.ServiceProvider);
            // }

            CreateHostBuilder(args).Build().Run();
        }


        // private static IServiceProvider CreateServices()
        // {
        //     return new ServiceCollection()
        //         // Add common FluentMigrator services
        //         .AddFluentMigratorCore()
        //         .ConfigureRunner(rb =>
        //         {
        //             var ret = rb
        //                                 // Add SQLite support to FluentMigrator
        //                                 .AddSQLite()
        //                                 // Set the connection string
        //                                 .WithGlobalConnectionString("Data Source=lb.db")
        //                                 // Define the assembly containing the migrations
        //                                 .ScanIn(typeof(Migrations.AddCustomersTable).Assembly).For.Migrations();
        //             System.Console.WriteLine("ret ... {0}", ret);

        //         })
        //         // Enable logging to console in the FluentMigrator way
        //         .AddLogging(lb => lb.AddFluentMigratorConsole())
        //         // Build the service provider
        //         .BuildServiceProvider(false);
        // }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
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
                });
        }
    }
}

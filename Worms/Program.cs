using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Worms.abstractions;
using ILogger = Worms.abstractions.ILogger;


namespace Worms
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }



        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services
                        .AddHostedService<SimulatorService>()
                        .AddScoped<IFoodGenerator, NormalFoodGenerator>(_ => 
                            new NormalFoodGenerator(new Random(), 0, 1, 10, 10))
                        .AddScoped<ILogger, Logger>(_ => 
                            new Logger(new StreamWriter("log.txt")))
                        .AddScoped<INameGenerator, JohnsNameGenerator>()
                        .AddScoped<IWormBehaviour, CircleWormBehaviour>(_ => 
                            new CircleWormBehaviour(new ActionFactory()));
                });
        }
    }

    
}
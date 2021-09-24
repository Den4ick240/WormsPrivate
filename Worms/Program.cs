using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Worms.abstractions;
using Worms.database;
using ILogger = Worms.abstractions.ILogger;


namespace Worms
{
    static class Program
    {
        private const int Ttl = 10;
        private const int FeedingPoints = 10;
        private static readonly NormalFoodGenerator NormalFoodGenerator =
            new NormalFoodGenerator(new Random(), 0, 1, Ttl, FeedingPoints);

        static void Main(string[] args)
        {
            using var dbCtx =
                new DatabaseContext(@"Server=localhost\SQLEXPRESS;Database=wormsBase;Trusted_Connection=True;");
            if (args.Length < 2)
            {
                using var streamWriter = new StreamWriter("log.txt");
                IFoodGenerator foodGenerator;
                if (args.Length == 0)
                {
                    foodGenerator = NormalFoodGenerator;
                }
                else
                {
                    var worldBehaviour = new DatabaseFoodReader(dbCtx).ReadWorldBehaviour(args[0]);
                    foreach (var worldBehaviourFoodPoint in worldBehaviour.FoodPoints)
                    {
                        Console.WriteLine(worldBehaviourFoodPoint.Order);
                    }
                    foodGenerator = new WorldBehaviourFoodGenerator(
                        worldBehaviour,
                        FeedingPoints, Ttl
                    );
                }

                CreateHostBuilder(args, streamWriter, foodGenerator).Build().Run();
            }
            else
            {
                GenerateWorldBehaviour(dbCtx, args[1]);
            }
        }

        private static void GenerateWorldBehaviour(DatabaseContext dbCtx, string behaviourName)
        {
            var worldBehaviour = new WorldBehaviourGenerator().Generate(behaviourName, NormalFoodGenerator);
            dbCtx.Add(worldBehaviour);
            dbCtx.SaveChanges();
        }


        private static IHostBuilder CreateHostBuilder(string[] args, TextWriter streamWriter,
            IFoodGenerator foodGenerator)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services
                        .AddHostedService<SimulatorService>()
                        .AddSingleton(_ => foodGenerator)
                        .AddSingleton<ILogger, Logger>(_ => new Logger(streamWriter))
                        .AddSingleton<INameGenerator, JohnsNameGenerator>()
                        .AddSingleton<IWormBehaviour, ClosestFoodWormBehaviour>(_ =>
                            new ClosestFoodWormBehaviour(new ActionFactory()));
                });
        }
    }
}
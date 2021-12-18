using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Worms.abstractions;
using Worms.database;
using Worms.database.entities;
using ILogger = Worms.abstractions.ILogger;


namespace Worms
{
    static class Program
    {
        private const int Ttl = 10;
        private const int FeedingPoints = 10;
        private const string LogFileName = "log.txt";

        private static readonly NormalFoodGenerator NormalFoodGenerator =
            new(new Random(), 0, 1, Ttl, FeedingPoints);

        private const string ConnectionString =
            @"Host=localhost;Port=5432;Database=worms;Username=postgres;Password=PostgressPassword09255432201222943";

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                CreateHostBuilder(args,
                        args.Length > 0,
                        args[0])
                    .Build()
                    .Run();
            }
            else
            {
                using var dbCtx = new DatabaseContext(ConnectionString);
                GenerateWorldBehaviour(dbCtx, args[1]);
            }
        }

        private static void GenerateWorldBehaviour(DatabaseContext dbCtx, string behaviourName)
        {
            var worldBehaviour = new WorldBehaviourGenerator().Generate(behaviourName, NormalFoodGenerator);
            dbCtx.Add(worldBehaviour);
            dbCtx.SaveChanges();
        }


        private static IHostBuilder CreateHostBuilder(string[] args,
            bool useDbFoodGenerator, string worldBehaviourName)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    if (useDbFoodGenerator)
                        services
                            .AddDbContext<
                                DatabaseContext>(optionsAction =>
                                optionsAction.UseNpgsql(
                                    ConnectionString))
                            .AddScoped<DatabaseFoodReader>()
                            .AddScoped(provider =>
                                provider.GetService<DatabaseFoodReader>().ReadWorldBehaviour(worldBehaviourName))
                            .AddScoped<IFoodGenerator>(provider =>
                                new WorldBehaviourFoodGenerator(
                                    provider.GetService<WorldBehaviour>(),
                                    FeedingPoints, Ttl));
                    else
                        services
                            .AddSingleton<IFoodGenerator>(_ => NormalFoodGenerator);
                    services
                        .AddScoped<TextWriter>(_ => new StreamWriter(LogFileName))
                        .AddHostedService<SimulatorService>()
                        .AddSingleton<ILogger, Logger>()
                        .AddSingleton<INameGenerator, JohnsNameGenerator>()
                        .AddSingleton<IWormBehaviour, ClosestFoodWormBehaviour>(_ =>
                            new ClosestFoodWormBehaviour(new ActionFactory()));
                });
        }
    }
}
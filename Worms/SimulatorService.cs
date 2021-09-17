using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Worms.abstractions;

namespace Worms
{
    public class SimulatorService : IHostedService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private readonly IFoodGenerator _foodGenerator;
        private readonly ILogger _logger;
        private readonly INameGenerator _nameGenerator;
        private readonly IWormBehaviour _wormBehaviour;

        public SimulatorService(
            IHostApplicationLifetime appLifetime,
            IFoodGenerator foodGenerator,
            ILogger logger,
            INameGenerator nameGenerator, 
            IWormBehaviour wormBehaviour)
        {
            _appLifetime = appLifetime;
            _foodGenerator = foodGenerator;
            _logger = logger;
            _nameGenerator = nameGenerator;
            _wormBehaviour = wormBehaviour;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(() =>
                Task.Run(async () =>
                {
                    RunSimulator();
                    _appLifetime.StopApplication();
                }));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void RunSimulator()
        {
            var sim = new Simulator(
                new WormFactory(10, _nameGenerator),
                _foodGenerator,
                _logger,
                _wormBehaviour
            );
            
            for (var i = 0; i < 100; i++)
            {
                // Thread.Sleep(10);
                Console.WriteLine("Simulator iteration: " + i);
                sim.RunFrame();
            }
        }
    }
}
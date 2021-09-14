using System;
using System.IO;

namespace Worms
{
    class Program
    {
        static void Main(string[] args)
        {
            using StreamWriter file = new("log.txt");
            new Simulator(
                new WormFactory(10, new JohnsNameGenerator(), new ActionFactory()),
                new FoodFactory(),
                new NormalFoodGenerator(new Random(), 0, 1),
                new Logger(file)
            ).Run();
        }
    }
}
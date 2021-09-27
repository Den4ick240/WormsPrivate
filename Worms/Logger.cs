using System.Collections.Generic;
using System.IO;
using System.Linq;
using Worms.abstractions;
using Worms.entities;

namespace Worms
{
    public class Logger : ILogger
    {
        private readonly TextWriter _writer;

        public Logger(TextWriter writer)
        {
            _writer = writer;
        }

        public void Log(World world)
        {
            WriteLine(GetWormsLog(world.Worms) + ", " + GetFoodsLog(world.Foods));
        }

        private void WriteLine(string line)
        {
            _writer.WriteLine(line);
        }

        private static string GetWormsLog(IEnumerable<Worm> worms)
        {
            return "Worms:[" +
                   string.Join(",",
                       worms.Select(
                           worm => worm.Name + "-" + worm.Ttl + " (" + worm.Point.X + "," + worm.Point.Y + ")"
                       )
                   ) + "]";
        }

        private static string GetFoodsLog(IEnumerable<Food> foods)
        {
            return "Foods: [" +
                   string.Join(",",
                       foods.Select(
                           food => "(" + food.Point.X + "," + food.Point.Y + ")")
                   ) + "]";
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Worms
{
    public class Logger
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

        private string GetWormsLog(List<AbstractWorm> worms)
        {
            return "Worms:[" +
                   string.Join(",",
                       worms.Select(
                           worm => worm.Name + "-" + worm.Ttl + " (" + worm.Point.X + "," + worm.Point.Y + ")"
                       )
                   ) + "]";
        }

        private string GetFoodsLog(List<Food> foods)
        {
            return "Foods: [" +
                   string.Join(",",
                       foods.Select(
                           food => "(" + food.Point.X + "," + food.Point.Y + ")")
                   ) + "]";
        }
    }
}
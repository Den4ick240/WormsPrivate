using System;
using System.Collections.Generic;
using Worms.abstractions;

namespace Worms
{
    public class NormalFoodGenerator : IFoodGenerator
    {
        private readonly Random _random;
        private readonly double _mu;
        private readonly double _sigma;
        private readonly int _ttl;
        private readonly int _feedingPoints;


        public NormalFoodGenerator(Random random, double mu, double sigma, int ttl, int feedingPoints)
        {
            _random = random;
            _sigma = sigma;
            _ttl = ttl;
            _feedingPoints = feedingPoints;
            _mu = mu;
        }

        public List<Food> GenerateFood(World world)
        {
            while (true)
            {
                Console.WriteLine("Trying to generate food...");
                var foodPoint = GetFoodPoint();
                if (world.Foods.Find(food => food.Point.Equals(foodPoint)) != null) continue;
                var food = new Food(_ttl, foodPoint, _feedingPoints);

                return new List<Food> {food};
            }
        }

        private Point GetFoodPoint()
        {
            return new Point(GetNextNormal(), GetNextNormal());
        }

        private int GetNextNormal()
        {
            var u1 = _random.NextDouble();
            var u2 = _random.NextDouble();
            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            var randNormal = _mu + _sigma * randStdNormal;
            return (int) Math.Round(randNormal);
        }
    }
}
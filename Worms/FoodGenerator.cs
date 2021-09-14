using System;
using System.Runtime.Intrinsics.X86;

namespace Worms
{
    public interface IFoodGenerator
    {
        Point GetFoodPoint();
    }

    public class NormalFoodGenerator : IFoodGenerator
    {
        private readonly Random _random;
        private readonly double _mu;
        private readonly double _sigma;

        public NormalFoodGenerator(Random random, double mu, double sigma)
        {
            _random = random;
            _sigma = sigma;
            _mu = mu;
        }

        public Point GetFoodPoint()
        {
            return new Point {X = GetNextNormal(), Y = GetNextNormal()};
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
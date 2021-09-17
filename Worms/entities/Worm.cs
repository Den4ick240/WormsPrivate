using Worms.abstractions;

namespace Worms.entities
{
    public class Worm
    {
        public int Ttl { get; private set; }
        public Point Point { get; set; }
        public string Name { get; }

        public Worm(int ttl, string name, Point point)
        {
            Ttl = ttl;
            Name = name;
            Point = point;
        }

        public void Feed(int points)
        {
            Ttl += points;
        }

        public void Age(int points = 1)
        {
            Ttl -= points;
        }

        public bool IsDead()
        {
            return Ttl <= 0;
        }
    }
}
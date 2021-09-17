using Worms.entities;

namespace Worms.abstractions
{
    public interface IWormFactory
    {
        public Worm GetWorm(Point point);
    }
}
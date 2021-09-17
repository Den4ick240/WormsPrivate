using Worms.abstractions;
using Worms.entities;

namespace Worms
{
    public class WormFactory : IWormFactory
    {
        private readonly int _ttl;
        private readonly INameGenerator _nameGenerator;

        public WormFactory(int ttl, INameGenerator nameGenerator)
        {
            _ttl = ttl;
            _nameGenerator = nameGenerator;
        }

        public Worm GetWorm(Point point)
        {
            return new Worm(_ttl, _nameGenerator.getNextName(), point);
        }
    }
}
using System.Collections.Generic;
using Worms.entities;

namespace Worms.abstractions
{
    public interface IWormAction
    {
        void perform(World world, Worm worm, List<Point> newWormPoints);
    }
}
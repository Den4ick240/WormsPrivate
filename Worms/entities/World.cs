using System.Collections.Generic;

namespace Worms.entities
{
    public class World
    {
        public List<Worm> Worms { init; get; }
        public List<Food> Foods { init; get; }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace Worms.Domain
{
    public class WorldState
    {
        public override string ToString()
        {
            Func<object, object, string> agg = (a, b) => a + ", " + b;
            return $"{nameof(Worms)}: [{Worms.Aggregate(agg)}], {nameof(Food)}: [{Food.Aggregate(agg)}]";
        }

        public List<Worm> Worms { get; set; }
        public List<Food> Food { get; set; }

        public Worm? FindWormAt(Position position)
        {
            return Worms.FirstOrDefault(w => w.Position.Equals(position));
        }
        
        public bool IsWormOnPosition(Position position)
        {
            return Worms.Any(w => w.Position.Equals(position));
        }

        public bool IsFoodOnPosition(Position position)
        {
            return Food.Any(f => f.Position.Equals(position));
        }
    }
}
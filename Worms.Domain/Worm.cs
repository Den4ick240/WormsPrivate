using System;

namespace Worms.Domain
{
    public class Worm
    {
        public string Name { get; set; }
        public int LifeStrength { get; set; }
        public Position Position { get; set; }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(LifeStrength)}: {LifeStrength}, {nameof(Position)}: {Position}";
        }
    }
}
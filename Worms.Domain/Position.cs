using System;

namespace Worms.Domain
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
        }

        public int Distance(Position position)
        {
            return Math.Abs(X - position.X) + Math.Abs(Y - position.Y);
        }
    }
}
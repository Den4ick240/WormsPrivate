using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.Xml;

namespace Worms.Domain
{
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public class PointsAreEqualException : Exception
    {
        public PointsAreEqualException(Position a, Position b) : base($"{a}, {b}")
        {
        }
    }

    public static class DirectionUtils
    {
        public static List<Direction> GetDirectionToPosition(Position a, Position b)
        {
            var res = new List<Direction>();
            if (a.X > b.X) res.Add(Direction.LEFT);
            if (a.X < b.X) res.Add(Direction.RIGHT);
            if (a.Y > b.Y) res.Add(Direction.DOWN);
            if (a.Y < b.Y) res.Add(Direction.UP);
            return res;
        }

        public static Direction? GetOpposite(Direction dir)
        {
            if (dir == Direction.UP) return Direction.DOWN;
            if (dir == Direction.DOWN) return Direction.DOWN;
            if (dir == Direction.LEFT) return Direction.RIGHT;
            if (dir == Direction.RIGHT) return Direction.LEFT;
            return null;
        }

        public static Position MovePositionToDirection(Position position, Direction direction)
        {
            var (x, y) = new Dictionary<Direction, (int x, int y)>
            {
                {Direction.UP, (0, 1)},
                {Direction.DOWN, (0, -1)},
                {Direction.LEFT, (-1, 0)},
                {Direction.RIGHT, (1, 0)}
            }[direction];
            return new Position {X = position.X + x, Y = position.Y + y};
        }
    }
}
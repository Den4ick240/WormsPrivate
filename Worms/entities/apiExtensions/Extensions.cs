using System.Collections.Generic;
using System.Linq;
using Worms.abstractions;
using Worms.Domain;

namespace Worms.entities.apiExtensions
{
    public static class Extensions
    {
        private static readonly Dictionary<Direction, Domain.Direction> Directions = new()
        {
            {Direction.Up, Domain.Direction.UP},
            {Direction.Down, Domain.Direction.DOWN},
            {Direction.Left, Domain.Direction.LEFT},
            {Direction.Right, Domain.Direction.RIGHT}
        };

        public static IWormAction GetActionFromApi(this ActionFactory actionFactory, Response response)
        {
            if (response.Direction == null)
                return actionFactory.GetDoNothingAction();
            var direction = ((Domain.Direction) response.Direction).fromApi();
            return response.Split
                ? actionFactory.GetReproduceAction(direction)
                : actionFactory.GetMoveAction(direction);
        }

        public static Direction fromApi(this Domain.Direction direction)
        {
            return Directions.FirstOrDefault(d => d.Value == direction).Key;
        }

        public static Domain.WorldState ToApi(this World world)
        {
            return new Domain.WorldState
            {
                Food = world.Foods.Select(f => f.ToApi()).ToList(),
                Worms = world.Worms.Select(w => w.ToApi()).ToList()
            };
        }

        public static Domain.Direction ToApi(this Direction direction)
        {
            return Directions[direction];
        }

        public static Domain.Position ToApi(this Point point)
        {
            return new Domain.Position {X = point.X, Y = point.Y};
        }

        public static Domain.Worm ToApi(this Worm worm)
        {
            return new Domain.Worm {LifeStrength = worm.Ttl, Name = worm.Name, Position = worm.Point.ToApi()};
        }

        public static Domain.Food ToApi(this Food food)
        {
            return new Domain.Food {ExpiresIn = food.Ttl, Position = food.Point.ToApi()};
        }
    }
}
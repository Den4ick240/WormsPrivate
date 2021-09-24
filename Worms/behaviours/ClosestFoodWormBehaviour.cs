using Worms.abstractions;
using Worms.entities;

namespace Worms
{
    public class ClosestFoodWormBehaviour : AbstractWormBehaviour
    {
        public ClosestFoodWormBehaviour(ActionFactory actionFactory) : base(actionFactory)
        {
        }

        public override IWormAction GetAction(World world, Worm worm)
        {
            Food closestFood = null;
            var minDistance = int.MaxValue;
            foreach (var food in world.Foods)
            {
                var distance = food.Point.Distance(worm.Point);
                if (distance >= minDistance || food.Ttl < distance || worm.Ttl < distance) continue;
                minDistance = distance;
                closestFood = food;
            }
            var direction = GetMoveToPointAction(
                worm.Point,
                closestFood == null
                    ? new Point(0, 0)
                    : closestFood.Point
            );
            return direction == null
                ? _actionFactory.GetDoNothingAction()
                : _actionFactory.GetMoveAction(direction.Value);
        }

        private static Direction? GetMoveToPointAction(Point wormPoint, Point point)
        {
            Direction? direction = null;
            if (wormPoint.X > point.X) direction = Direction.Left;
            if (wormPoint.X < point.X) direction = Direction.Right;
            if (wormPoint.Y < point.Y) direction = Direction.Down;
            if (wormPoint.Y > point.Y) direction = Direction.Up;
            return direction;
        }
    }
}
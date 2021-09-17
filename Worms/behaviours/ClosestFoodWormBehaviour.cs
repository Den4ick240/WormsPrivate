using System;
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
            int minDistance = int.MaxValue;
            foreach (var food in world.Foods)
            {
                int distance = food.Point.Distance(worm.Point);
                if (distance < minDistance && food.Ttl >= distance && worm.Ttl >= distance)
                {
                    minDistance = distance;
                    closestFood = food;
                }
            }

            return GetMoveToPointAction(
                worm.Point,
                closestFood == null
                    ? new Point(0,0) 
                    : closestFood.Point
            );
        }

        private IWormAction GetMoveToPointAction(Point wormPoint, Point point)
        {
            Direction? direction = null;
            if (wormPoint.X > point.X) direction = Direction.Left;
            if (wormPoint.X < point.X) direction = Direction.Right;
            if (wormPoint.Y < point.Y) direction = Direction.Down;
            if (wormPoint.Y > point.Y) direction = Direction.Up;
            return direction == null
                ? _actionFactory.GetDoNothingAction()
                : _actionFactory.GetMoveAction((Direction) direction);
        }
    }
}
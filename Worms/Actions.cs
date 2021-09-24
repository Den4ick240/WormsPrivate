using System.Collections.Generic;
using Worms.abstractions;
using Worms.entities;

namespace Worms
{
    public class DoNothingAction : IWormAction
    {
        public void perform(World world, Worm worm, List<Point> newWormPoints)
        {
        }
    }

    public class MoveAction : IWormAction
    {
        public MoveAction()
        {
        }

        public MoveAction(Direction direction)
        {
            Direction = direction;
        }

        public Direction Direction { init; get; }

        public void perform(World world, Worm worm, List<Point> newWormPoints)
        {
            var newPoint = worm.Point.Move(Direction);
            if (world.Worms.Find(worm => worm.Point.Equals(newPoint)) == null)
                worm.Point = newPoint;
        }

        public override string ToString()
        {
            return "move " + Direction;
        }
    }

    public class ReproduceAction : IWormAction
    {
        public Direction Direction { init; get; }

        public void perform(World world, Worm worm, List<Point> newWormPoints)
        {
            var newPoint = worm.Point.Move(Direction);
            var freeFromWorms = world.Worms.Find(worm => worm.Point.Equals(newPoint)) == null;
            var free = freeFromWorms &&
                       world.Foods.Find(food => food.Point.Equals(newPoint)) == null &&
                       newWormPoints.Find(point => point.Equals(newPoint)) == null;
            if (free)
            {
                newWormPoints.Add(newPoint);
            }
        }
    }

    public class ActionFactory
    {

        public IWormAction GetMoveAction(Direction direction)
        {
            return new MoveAction {Direction = direction};
        }

        public IWormAction GetReproduceAction(Direction direction)
        {
            return new ReproduceAction {Direction = direction};
        }

        public IWormAction GetDoNothingAction()
        {
            return new DoNothingAction();
        }
    }
}
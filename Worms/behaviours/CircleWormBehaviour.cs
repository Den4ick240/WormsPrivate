using Worms.abstractions;
using Worms.entities;

namespace Worms
{
    public class CircleWormBehaviour : AbstractWormBehaviour
    {
        private int _moveCounter;

        public CircleWormBehaviour(ActionFactory actionFactory) : base(actionFactory)
        {
        }

        public override IWormAction GetAction(World world, Worm worm)
        {
            var arr = new[]
            {
                Direction.Right,
                Direction.Right,
                Direction.Down,
                Direction.Down,
                Direction.Left,
                Direction.Left,
                Direction.Up,
                Direction.Up,
            };
            var res =  _moveCounter == 0
                ? _actionFactory.GetMoveAction(Direction.Up)
                : _actionFactory.GetMoveAction(arr[_moveCounter % arr.Length]);
            _moveCounter++;
            return res;
        }
    }
}
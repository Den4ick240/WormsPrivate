using Worms.abstractions;

namespace Worms.entities
{
    // public class CircleWorm : AbstractWorm
    // {
    //     private int _moveCounter;
    //     public CircleWorm(int ttl, string name, ActionFactory actionFactory) : base(ttl, name, actionFactory)
    //     {
    //     }
    //
    //     public override void Age(int points = 1)
    //     {
    //         base.Age(points);
    //         _moveCounter++;
    //     }
    //
    //     public override IWormAction GetAction(World world)
    //     {
    //         var arr = new[]
    //         {
    //             Direction.Right,
    //             Direction.Right,
    //             Direction.Down,
    //             Direction.Down,
    //             Direction.Left,
    //             Direction.Left,
    //             Direction.Up,
    //             Direction.Up,
    //         };
    //         return _moveCounter == 0
    //             ? _actionFactory.GetMoveAction(Direction.Up)
    //             : _actionFactory.GetMoveAction(arr[_moveCounter % arr.Length]);
    //         
    //     }
    // }
}
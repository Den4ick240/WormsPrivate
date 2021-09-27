using Worms.entities;

namespace Worms.abstractions
{
    public interface IWormBehaviour
    {
        public IWormAction GetAction(World world, Worm worm);
    }

    public abstract class AbstractWormBehaviour : IWormBehaviour
    {
        protected readonly ActionFactory _actionFactory;

        protected AbstractWormBehaviour(ActionFactory actionFactory)
        {
            _actionFactory = actionFactory;
        }

        public abstract IWormAction GetAction(World world, Worm worm);
    }
}
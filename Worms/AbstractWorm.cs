namespace Worms
{
    public abstract class AbstractWorm
    {
        protected readonly ActionFactory _actionFactory;
        public int Ttl { get; private set; }
        public Point Point { get; set; }
        public string Name { get; }

        protected AbstractWorm(int ttl, string name, ActionFactory actionFactory)
        {
            Ttl = ttl;
            Name = name;
            _actionFactory = actionFactory;
        }

        public virtual void Feed(int points)
        {
            Ttl += points;
        }

        public virtual void Age(int points = 1)
        {
            Ttl -= points;
        }

        public virtual bool IsDead()
        {
            return Ttl <= 0;
        }

        public abstract IWormAction GetAction(World world);
    }

    public class CircleWorm : AbstractWorm
    {
        private int _moveCounter;
        public CircleWorm(int ttl, string name, ActionFactory actionFactory) : base(ttl, name, actionFactory)
        {
        }

        public override void Age(int points = 1)
        {
            base.Age(points);
            _moveCounter++;
        }

        public override IWormAction GetAction(World world)
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
            return _moveCounter == 0
                ? _actionFactory.GetMoveAction(Direction.Up)
                : _actionFactory.GetMoveAction(arr[_moveCounter % arr.Length]);
            
        }
    }
    
    public class WormFactory
    {
        private readonly int _ttl;
        private readonly INameGenerator _nameGenerator;
        private readonly ActionFactory _actionFactory;

        public WormFactory(int ttl, INameGenerator nameGenerator, ActionFactory actionFactory)
        {
            _ttl = ttl;
            _nameGenerator = nameGenerator;
            _actionFactory = actionFactory;
        }

        public AbstractWorm GetWorm(Point point)
        {
            return new CircleWorm(_ttl, _nameGenerator.getNextName(), _actionFactory) {Point = point};
        }
    }
}
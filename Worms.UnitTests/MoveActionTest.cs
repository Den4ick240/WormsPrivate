using System.Collections.Generic;
using NUnit.Framework;
using Worms.abstractions;
using Worms.entities;

namespace Worms.UnitTests
{
    public class MoveActionTest
    {
        private readonly IWormFactory _wormFactory = new MockWormFactory();

        [Test]
        public void performTest_freeCell_moves()
        {
            var dict = new Dictionary<Direction, Point>
            {
                {Direction.Up, new Point(1, 0)},
                {Direction.Down, new Point(1, 2)},
                {Direction.Right, new Point(2, 1)},
                {Direction.Left, new Point(0, 1)}
            };

            foreach (var (direction, point) in dict)
            {
                const string name = "John";
                var world = new World {Worms = new List<Worm>(), Foods = new List<Food>()};
                var worm = new Worm(10, name, new Point(1, 1));
                world.Worms.Add(worm);
                var testWormBehaviour = new TestWormBehaviour(
                    new Dictionary<string, IWormAction> {{name, new MoveAction(direction)}}
                );
                var sim = new Simulator(_wormFactory, new MockFoodGenerator(), new MockLogger(),
                    testWormBehaviour, world);

                sim.RunFrame();

                Assert.AreEqual(worm.Point, point);
            }
        }

        [Test]
        public void perform_wormCell_stays()
        {
            var world = new World {Worms = new List<Worm>(), Foods = new List<Food>()};
            var worm1Point = new Point(0, 0);
            var worm2Point = new Point(0, 1);
            var worm1 = new Worm(10, "1", worm1Point);
            var worm2 = new Worm(10, "2", worm2Point);

            var behaviour = new TestWormBehaviour(
                new Dictionary<string, IWormAction>
                {
                    {"1", new MoveAction(Direction.Down)},
                    {"2", new DoNothingAction()}
                });

            var sim = new Simulator(_wormFactory, new MockFoodGenerator(),
                new MockLogger(), behaviour, world);

            sim.RunFrame();

            Assert.AreEqual(worm1.Point, worm1Point);
            Assert.AreEqual(worm2.Point, worm2Point);
        }

        [Test]
        public void performTest_foodCell_eats()
        {
            var foodPoint = new Point(0, 1);
            var wormPoint = new Point(0, 0);
            const string name = "1";
            const int feedingPoints = 10;
            const int wormTtl = 10;
            var actions = new Dictionary<string, IWormAction> {{name, new MoveAction(Direction.Down)}};
            var food = new Food(10, foodPoint, feedingPoints);
            Worm worm = new(wormTtl, name, wormPoint);

            var world = new World
            {
                Foods = new List<Food> {food}, Worms = new List<Worm> {worm}
            };

            var sim = new Simulator(_wormFactory, new MockFoodGenerator(), new MockLogger(),
                new TestWormBehaviour(actions), world);

            sim.RunFrame();

            Assert.AreEqual(worm.Point, foodPoint);
            Assert.IsEmpty(world.Foods);
            Assert.AreEqual(worm.Ttl, wormTtl + feedingPoints - 1);
        }
    }
}
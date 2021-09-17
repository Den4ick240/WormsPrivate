using System.Collections.Generic;
using NUnit.Framework;
using Worms.abstractions;
using Worms.entities;

namespace Worms.UnitTests
{
    public class ReproduceActionTest
    {
        private readonly IWormFactory _wormFactory = new WormFactory(10, new JohnsNameGenerator(""));
        private const Direction Direction = Worms.Direction.Down;
        private const string firstWormName = "1";
        private const string secondWormName = "2";

        private readonly IWormBehaviour _behaviour = new TestWormBehaviour(
            new Dictionary<string, IWormAction>
            {
                {firstWormName, new ReproduceAction {Direction = Direction}},
                {secondWormName, new DoNothingAction()}
            }
        );

        private readonly Point _worm1Point = new(0, 0);
        private readonly Point _worm2Point = new(0, 1);

        [Test]
        public void perform_freeCell_reproduces()
        {
            var worm = _wormFactory.GetWorm(_worm1Point);
            var world = new World
            {
                Foods = new List<Food>(),
                Worms = new List<Worm> {worm}
            };
            var sim = new Simulator(_wormFactory, new MockFoodGenerator(), new MockLogger(), _behaviour, world);

            sim.RunFrame();

            Assert.AreEqual(worm.Point, _worm1Point);
            Assert.AreEqual(world.Worms.Count, 2);
            var worm1 = world.Worms[0];
            var worm2 = world.Worms[1];
            Assert.AreEqual(worm1, worm);
            Assert.AreEqual(worm2.Point, _worm2Point);
        }

        [Test]
        public void perform_wormCell_stays()
        {
            var worm = new Worm(10, firstWormName, _worm1Point);
            var worm2 = new Worm(10, secondWormName, _worm2Point);
            var world = new World {Foods = new List<Food>(), Worms = new List<Worm> {worm, worm2}};
            var sim = new Simulator(_wormFactory, new MockFoodGenerator(), new MockLogger(), _behaviour, world);

            sim.RunFrame();

            Assert.AreEqual(worm.Point, _worm1Point);
            Assert.AreEqual(world.Worms.Count, 2);
            Assert.IsTrue(world.Worms.Contains(worm));
        }

        [Test]
        public void perform_foodCell_stays()
        {
            var worm = new Worm(10, firstWormName, _worm1Point);
            var food = new Food(10, _worm2Point, 10);
            var world = new World
            {
                Foods = new List<Food> {food}, Worms = new List<Worm> {worm}
            };
            var sim = new Simulator(_wormFactory, new MockFoodGenerator(), new MockLogger(), _behaviour, world);
            
            sim.RunFrame();

            Assert.AreEqual(worm.Point, _worm1Point);
            Assert.IsTrue(world.Foods.Contains(food));
            Assert.AreEqual(world.Worms.Count, 1);
            Assert.IsTrue(world.Worms.Contains(worm));
        }
    }
}
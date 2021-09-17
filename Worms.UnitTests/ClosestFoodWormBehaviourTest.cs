using System.Collections.Generic;
using NUnit.Framework;
using Worms.abstractions;
using Worms.entities;

namespace Worms.UnitTests
{
    public class ClosestFoodWormBehaviourTest
    {
        [Test]
        public void closestFoodTest()
        {
            var points = new[]
            {
                new {wormPoint = new Point(1, 1), foodPoint = new Point(3, -4)},
                new {wormPoint = new Point(1, -1), foodPoint = new Point(-2, -4)},
                new {wormPoint = new Point(2, 0), foodPoint = new Point(3, -6)},
            };

            const int wormTtl = 10;
            const int feedingPoints = 10;
            foreach (var point in points)
            {
                var worm = new Worm(wormTtl, "", point.wormPoint);
                var food = new Food(wormTtl, point.foodPoint, feedingPoints);
                var world = new World {Foods = new List<Food> {food}, Worms = new List<Worm> {worm}};
                var sim = new Simulator(new MockWormFactory(), new MockFoodGenerator(), new MockLogger(),
                    new ClosestFoodWormBehaviour(new ActionFactory()), world);


                var distance = point.wormPoint.Distance(point.foodPoint);
                for (var i = 0; i < distance; i++)
                {
                    sim.RunFrame();
                }
                
                Assert.AreEqual(point.foodPoint, worm.Point);
                Assert.AreEqual(feedingPoints + wormTtl - distance, worm.Ttl);
                Assert.IsEmpty(world.Foods);
            }
        }
    }
}
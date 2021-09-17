using System;
using System.Collections.Generic;
using NUnit.Framework;
using Worms.abstractions;
using Worms.entities;

namespace Worms.UnitTests
{
    public class FoodGenerationTest
    {
        private readonly List<IFoodGenerator> _foodGenerators = new()
        {
            new NormalFoodGenerator(new Random(), 0, 1, 10, 10)
        };

        private const int RequiredFoodGenerationNumber = 40;

        [Test]
        public void uniqueFoodTest()
        {
            foreach (var foodGenerator in _foodGenerators)
            {
                var world = new World {Foods = new List<Food>(), Worms = new List<Worm>()};
                for (var i = 0; i < RequiredFoodGenerationNumber; i++)
                {
                    foodGenerator.GenerateFood(world);
                }

                for (var i = 0; i < world.Foods.Count; i++)
                {
                    for (var j = i + 1; j < world.Foods.Count; j++)
                    {
                        Assert.AreNotEqual(world.Foods[i].Point, world.Foods[j].Point);
                    }
                }
            }
        }


        [Test]
        public void foodGeneratedOnWormTest()
        {
            var point = new Point(1, 1);
            var foodGenerator = new OneFoodGenerator(point);
            var worm = new Worm(10, "", point);

            var world = new World {Foods = new List<Food>(), Worms = new List<Worm> {worm}};
            var sim = new Simulator(new MockWormFactory(), foodGenerator, new MockLogger(),
                new TestWormBehaviour(), world);
            
            sim.RunFrame();
            
            Assert.AreEqual(point, worm.Point);
            Assert.IsEmpty(world.Foods);
            Assert.AreEqual(19, worm.Ttl);
        }
    }
}
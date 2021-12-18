using System;
using System.Linq;
using NUnit.Framework;
using Worms.database;

namespace Worms.UnitTests
{
    public class DatabaseContextTest
    {
        [Test]
        public void AddAndReadWorldBehaviourTest()
        {
            const string name = "name";
            var worldBehaviour =
                new WorldBehaviourGenerator()
                    .Generate(name, new NormalFoodGenerator(new Random(), 0, 1, 10, 10));
            var dbCtx = new DatabaseContext((string) null);
            dbCtx.WorldBehaviours.Add(worldBehaviour);
            dbCtx.SaveChanges();
            var retrievedWorldBehaviour = dbCtx.WorldBehaviours.First(behaviour => behaviour.Name == name);
            Assert.AreEqual(worldBehaviour, retrievedWorldBehaviour);
        }
        
    }
}
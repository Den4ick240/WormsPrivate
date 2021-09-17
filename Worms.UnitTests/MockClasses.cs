using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Transactions;
using Worms.abstractions;
using Worms.entities;

namespace Worms.UnitTests
{
    public class MockLogger : ILogger
    {
        public void Log(World world)
        {
        }
    }

    public class MockNameGenerator : INameGenerator
    {
        public MockNameGenerator(string name)
        {
            Name = name;
        }

        private string Name { get; }
        public string getNextName()
        {
            return Name;
        }
    }

    public class OneFoodGenerator : IFoodGenerator
    {
        private readonly Point _point;
        private bool _generated = false;

        public OneFoodGenerator(Point point)
        {
            _point = point;
        }

        public List<Food> GenerateFood(World world)
        {
            if (_generated) return new List<Food>();
            var food = new Food(10, _point, 10);
            return new List<Food> {food};
        }
    }

    public class MockFoodGenerator : IFoodGenerator
    {
        public List<Food> GenerateFood(World world)
        {
            return new List<Food>();
        }
    }

    public class TestWormBehaviour : IWormBehaviour
    {
        private readonly Dictionary<string, IWormAction> _actions;

        public TestWormBehaviour(Dictionary<string, IWormAction> actions = null)
        {
            _actions = actions;
        }

        public IWormAction GetAction(World world, Worm worm)
        {
            return _actions != null
                ? _actions[worm.Name]
                : new DoNothingAction();
        }
    }

    public class MockWormFactory : IWormFactory
    {
        public Worm GetWorm(Point point)
        {
            throw new NotImplementedException();
        }
    }
}
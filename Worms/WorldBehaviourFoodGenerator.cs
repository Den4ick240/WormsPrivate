using System;
using System.Collections.Generic;
using System.Linq;
using Worms.abstractions;
using Worms.database.entities;

namespace Worms
{
    public class WorldBehaviourFoodGenerator : IFoodGenerator
    {
        private readonly int _ttl;
        private readonly int _feedingPoints;
        private readonly WorldBehaviour _worldBehaviour;
        private int _moveCounter = 0;

        public WorldBehaviourFoodGenerator(WorldBehaviour worldBehaviour, int feedingPoints, int ttl)
        {
            _worldBehaviour = worldBehaviour;
            _feedingPoints = feedingPoints;
            _ttl = ttl;
        }

        public List<Food> GenerateFood(World world)
        {
            var list = _worldBehaviour
                .FoodPoints
                .Where(point => point.Order == _moveCounter)
                .Select(point =>
                    new Food(
                        _ttl,
                        new Point(point.X, point.Y),
                        _feedingPoints)
                )
                .ToList();
            _moveCounter++;
            return list;
        }
    }
}
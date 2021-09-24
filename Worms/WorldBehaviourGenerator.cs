using System.Collections.Generic;
using System.Linq;
using Worms.abstractions;
using Worms.database.entities;
using Worms.entities;

namespace Worms
{
    public class WorldBehaviourGenerator
    {
        public WorldBehaviour Generate(string name, IFoodGenerator foodGenerator, int count = 100)
        {
            var worldBehaviour = new WorldBehaviour
            {
                Name = name,
                FoodPoints = GenerateFoodPoints(foodGenerator, count)
            };
            return worldBehaviour;
        }

        private ICollection<FoodPoint> GenerateFoodPoints(IFoodGenerator foodGenerator, int count)
        {
            var collection = new List<FoodPoint>();
            var world = new World {Foods = new List<Food>(), Worms = new List<Worm>()};
            for (var i = 0; i < count; i++)
            {
                var foods = foodGenerator.GenerateFood(world);
                world.Foods.AddRange(foods);
                collection.AddRange(foods.Select(food => new FoodPoint
                    {Order = i, X = food.Point.X, Y = food.Point.Y}));
                foreach (var worldFood in world.Foods)
                {
                    worldFood.Age();
                }

                world.Foods.RemoveAll(food => food.IsExpired());
            }

            return collection;
        }
    }
}
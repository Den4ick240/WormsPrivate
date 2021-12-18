using System;
using System.Collections.Generic;
using Worms.abstractions;
using Worms.entities;

namespace Worms
{
    public class Simulator
    {
        private readonly IWormFactory _wormFactory;
        private readonly IFoodGenerator _foodGenerator;
        private readonly List<Worm> _worms;
        private readonly List<Food> _foods;
        private readonly World _world;
        private readonly ILogger _logger;
        private readonly IWormBehaviour _wormBehaviour;

        public Simulator(
            IWormFactory wormFactory,
            IFoodGenerator foodGenerator,
            ILogger logger,
            IWormBehaviour wormBehaviour
        )
        {
            _wormFactory = wormFactory;
            _foodGenerator = foodGenerator;
            _logger = logger;
            _wormBehaviour = wormBehaviour;
            _worms = new List<Worm>();
            _foods = new List<Food>();
            _world = new World {Worms = _worms, Foods = _foods};
            _worms.Add(wormFactory.GetWorm(new Point(0, 0)));
        }

        public Simulator(
            IWormFactory wormFactory,
            IFoodGenerator foodGenerator,
            ILogger logger,
            IWormBehaviour wormBehaviour,
            World world
        )
        {
            _wormFactory = wormFactory;
            _foodGenerator = foodGenerator;
            _logger = logger;
            _wormBehaviour = wormBehaviour;
            _worms = world.Worms;
            _foods = world.Foods;
            _world = world;
        }

        public void RunFrame()
        {
            GenerateFood();
            _logger.Log(_world);
            MoveWorms();
            Console.WriteLine("worms moved");
            FeedWorms();
            Console.WriteLine("worms fed");
            AgeFoods();
            Console.WriteLine("foods aged");
            DeleteDeadWorms();
            Console.WriteLine("dead worms buried");
        }

        private void FeedWorms()
        {
            foreach (var worm in _worms)
            {
                var food = _foods.Find(food => food.Point.Equals(worm.Point));
                if (food != null)
                {
                    worm.Feed(food.FeedingPoints);
                    _foods.Remove(food);
                }
            }
        }

        private void AgeFoods()
        {
            foreach (var food in _foods)
            {
                food.Age();
            }

            _foods.RemoveAll(food => food.IsExpired());
        }

        private void MoveWorms()
        {
            var newWormPoints = new List<Point>();
            foreach (var worm in _worms)
            {
                var action = _wormBehaviour.GetAction(_world, worm);
                Console.WriteLine(worm.Name + " wants to " + action);
                action.perform(_world, worm, newWormPoints);
                worm.Age();
            }
            foreach (var newWormPoint in newWormPoints)
            {
                _worms.Add(_wormFactory.GetWorm(newWormPoint));
            }
        }

        private void DeleteDeadWorms()
        {
            _worms.RemoveAll(worm => worm.IsDead());
        }

        private void GenerateFood()
        {
            foreach (var food in _foodGenerator.GenerateFood(_world))
            {
                PlaceFood(food);
            }
        }

        private void PlaceFood(Food food)
        {
            var worm = _worms.Find(worm => worm.Point.Equals(food.Point));
            if (worm == null)
                _foods.Add(food);
            else
            {
                Console.WriteLine("Feeding");
                Console.WriteLine(worm.Ttl);
                worm.Feed(food.FeedingPoints);
                Console.WriteLine(worm.Ttl);
            }
        }
    }
}
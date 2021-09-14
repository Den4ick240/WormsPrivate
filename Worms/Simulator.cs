using System;
using System.Collections.Generic;
using System.Threading;

namespace Worms
{
    public class Simulator
    {
        private readonly WormFactory _wormFactory;
        private readonly FoodFactory _foodFactory;
        private readonly IFoodGenerator _foodGenerator;
        private readonly List<AbstractWorm> _worms;
        private readonly List<Food> _foods;
        private readonly World _world;
        private readonly Logger _logger;

        public Simulator(WormFactory wormFactory, FoodFactory foodFactory, IFoodGenerator foodGenerator, Logger logger)
        {
            _wormFactory = wormFactory;
            _foodFactory = foodFactory;
            _foodGenerator = foodGenerator;
            _worms = new List<AbstractWorm>();
            _foods = new List<Food>();
            _world = new World {Worms = _worms, Foods = _foods};
            _logger = logger;
            _worms.Add(wormFactory.GetWorm(new Point {X = 0, Y = 0}));
        }

        private void RunFrame()
        {
            GenerateFood();
            _logger.Log(_world);
            MoveWorms();
            FeedWorms();
            AgeFoods();
            DeleteDeadWorms();
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
                var action = worm.GetAction(_world);
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
            while (true)
            {
                Console.WriteLine("Trying to generate food...");
                var foodPoint = _foodGenerator.GetFoodPoint();
                if (_foods.Find(food => food.Point.Equals(foodPoint)) != null) continue;
                PlaceFood(_foodFactory.GetFood(foodPoint));
                Console.WriteLine("Food generated");
                return;
            }
        }

        private void PlaceFood(Food food)
        {
            var worm = _worms.Find(worm => worm.Point.Equals(food.Point));
            worm?.Feed(food.FeedingPoints);
            _foods.Add(food);
        }

        public void Run()
        {
            for (var i = 0; i < 100; i++)
            {
                Thread.Sleep(10);
                    Console.WriteLine("Simulator iteration: " + i);
                RunFrame();
            }
        }
    }
}
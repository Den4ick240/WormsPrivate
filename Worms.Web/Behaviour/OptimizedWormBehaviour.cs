﻿using System;
using System.Collections.Generic;
using System.Linq;
using Worms.Domain;

namespace Worms.Web.Behaviour
{
    public class OptimizedWormBehaviour : AbstractWormBehaviour
    {
        protected override Response GetResponse(WorldState worldState, Worm worm, int run, int step)
        {
            var stepsLeft = 100 - step;
            if (stepsLeft + 11 < worm.LifeStrength && stepsLeft < 10)
            {
                var directionToSplit = FindFreeDirection(worldState, worm.Position, Direction.UP);
                if (directionToSplit != null) return new Response(directionToSplit.GetValueOrDefault(), true);
            }
            
            List<(Worm worm, List<(int dist, Food food)> availableFoods)>
                wormsWithFoods =
                    worldState.Worms.Select(worldStateWorm =>
                        (worldStateWorm,
                            FindAvailableFoods(worldState, worldStateWorm.Position, worldStateWorm.LifeStrength))
                    ).ToList();

            for (var i = -1; i < wormsWithFoods.Count - 1; i++)
            {
                var updated = true;
                while (updated)
                {
                    updated = false;
                    List<Food> highlyDemandedFoods = (
                        from wormWithFood in (i > 1
                            ? wormsWithFoods.GetRange(i - 1, wormsWithFoods.Count - i)
                            : wormsWithFoods)
                        where wormWithFood.availableFoods.Count == 1
                        select wormWithFood.availableFoods[0].food
                    ).ToList();

                    foreach (var highlyDemandedFood in highlyDemandedFoods)
                    {
                        var wormsWithManyAvailableFoods =
                            wormsWithFoods.Where(wormWithFood => wormWithFood.availableFoods.Count > 1);
                        updated = wormsWithManyAvailableFoods
                            .Aggregate(updated,
                                (current, wormWithFood) =>
                                    current && 0 !=
                                    wormWithFood.availableFoods.RemoveAll(p => p.food.Equals(highlyDemandedFood)));
                    }

                    if (i < 0) continue;

                    var end = wormsWithFoods[i].availableFoods.Count - 1;
                    if (end >= 1)
                        wormsWithFoods[i].availableFoods.RemoveRange(1, end);
                }
            }

            var directionToPosition = DirectionUtils.GetDirectionToPosition(worm.Position, new Position {X = 0, Y = 0});
            var responseToCenter =
                directionToPosition.Count == 0 ? new Response() : new Response(directionToPosition[0]);
            var (_, availableFoods) = wormsWithFoods.Find(p => p.worm.Equals(worm));
            if (availableFoods.Count == 0)
            {
                return responseToCenter;
            }


            var directions = DirectionUtils.GetDirectionToPosition(worm.Position,
                availableFoods[0].food.Position);
            if (directions.Count == 0)
                return responseToCenter;

            var direction = directions[0];
            directions.ForEach(d =>
                direction =
                    !worldState.IsWormOnPosition(DirectionUtils.MovePositionToDirection(worm.Position, d))
                        ? d
                        : direction
            );


            if (worm.LifeStrength > 40 + availableFoods[0].dist)
            {
                var splitDirection = FindFreeDirection(worldState, worm.Position, direction);
                if (splitDirection != null)
                    return new Response(DirectionUtils.GetOpposite(splitDirection.GetValueOrDefault()), true);
            }


            return new Response(direction);
        }

        private static Direction? FindFreeDirection(WorldState worldState, Position position, Direction direction)
        {
            var dirs = new[] {Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT};
            var i = Array.FindIndex(dirs, d => d.Equals(direction));
            for (var j = (i + 1) % 4; j != i; j = (j + 1) % 4)
            {
                var pos = DirectionUtils.MovePositionToDirection(position, dirs[j]);
                if (!worldState.IsFoodOnPosition(pos) && !worldState.IsWormOnPosition(pos))
                {
                    return dirs[j];
                }
            }

            return null;
        }

        private static List<(int, Food)> FindAvailableFoods(
            WorldState worldState,
            Position wormPosition,
            int wormLifeStrength)
        {
            var list = (
                    from food in worldState.Food
                    let dist = food.Position.Distance(wormPosition)
                    where dist < wormLifeStrength && dist < food.ExpiresIn
                    select (dist, food)
                )
                .ToList();
            list.Sort((a, b) => a.dist - b.dist);
            return list;
        }
    }
}
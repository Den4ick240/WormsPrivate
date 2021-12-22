using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Localization;
using Worms.Domain;

namespace Worms.Web.Behaviour
{
    public class OptimizedWormBehaviour : AbstractWormBehaviour
    {
        private const int TotalSteps = 100;
        private const int LifeStrengthToSplit = 11;

        protected override Response GetResponse(WorldState worldState, Worm worm, int run, int step)
        {
            var stepsLeftUntilEnd = TotalSteps - step;
            var wontDieUntilEndIfSplit = stepsLeftUntilEnd + LifeStrengthToSplit < worm.LifeStrength;
            var childWontDieUntilEndIfSplit = stepsLeftUntilEnd < 10;
            if (wontDieUntilEndIfSplit && childWontDieUntilEndIfSplit)
            {
                var freeDirectionToSplit = FindFreeDirectionToSplit(worldState, worm.Position);
                if (freeDirectionToSplit != null) return new Response(freeDirectionToSplit.GetValueOrDefault(), true);
            }

            List<(Worm worm, List<(int dist, Food food)> availableFoods)> wormsWithFoods =
                worldState.Worms.Select(worldStateWorm =>
                    (
                        worldStateWorm,
                        FindAvailableFoodsWithDistance(worldState, worldStateWorm.Position, worldStateWorm.LifeStrength)
                    )
                ).ToList();

            for (var i = -1; i < wormsWithFoods.Count - 1; i++)
            {
                var updated = true;
                while (updated)
                {
                    updated = false;
                    List<Food> highlyDemandedFoods = (
                        from wormWithFood in
                            i > 1
                                ? wormsWithFoods.GetRange(i - 1, wormsWithFoods.Count - i)
                                : wormsWithFoods
                        where wormWithFood.availableFoods.Count == 1
                        select wormWithFood.availableFoods[0].food
                    ).ToList();


                    foreach (var highlyDemandedFood in highlyDemandedFoods)
                    {
                        // var min = wormsWithFoods
                            // .Where(wormWithFood =>
                                // wormWithFood.availableFoods.Count == 1 &&
                                // wormWithFood.availableFoods[0].food.Equals(highlyDemandedFood))
                            // .Min(wormWithFood => wormWithFood.worm.LifeStrength);

                        var wormsWithManyAvailableFoods =
                            wormsWithFoods.Where(wormWithFood => wormWithFood.availableFoods.Count > 1 
                                                                 // || wormWithFood.worm.LifeStrength > min
                                                                 );
                        updated = wormsWithManyAvailableFoods
                            .Aggregate(updated,
                                (current, wormWithFood) =>
                                    current && 0 !=
                                    wormWithFood.availableFoods.RemoveAll(p => p.food.Equals(highlyDemandedFood)));
                    }
                }


                if (i < 0) continue;

                var end = wormsWithFoods[i].availableFoods.Count - 1;
                if (end >= 1)
                    wormsWithFoods[i].availableFoods.RemoveRange(1, end);
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


            if (worm.LifeStrength > 50 + availableFoods[0].dist)
            {
                var splitDirection = FindFreeDirectionToSplit(worldState, worm.Position, direction);
                if (splitDirection != null)
                    return new Response(DirectionUtils.GetOpposite(splitDirection.GetValueOrDefault()), true);
            }


            return new Response(direction);
        }

        private static Direction? FindFreeDirectionToSplit(WorldState worldState, Position position,
            Direction directionToAvoid = Direction.UP)
        {
            var directionArray = new[] {Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT};

            //You might not want to split to direction which you are going to go on the next step, because the child might get in the way
            //so we try to split to another direction, if possible
            var directionIndexToAvoid = Array.FindIndex(directionArray, d => d.Equals(directionToAvoid));

            for (var i = (directionIndexToAvoid + 1) % 4; i != directionIndexToAvoid; i = (i + 1) % 4)
            {
                var currentDirection = directionArray[i];
                var positionMovedToDirection = DirectionUtils.MovePositionToDirection(position, currentDirection);
                if (!worldState.IsFoodOnPosition(positionMovedToDirection) &&
                    !worldState.IsWormOnPosition(positionMovedToDirection))
                {
                    return currentDirection;
                }
            }

            //null means there's no direction that you can split to
            return null;
        }

        private static List<(int, Food)> FindAvailableFoodsWithDistance(
            WorldState worldState,
            Position wormPosition,
            int wormLifeStrength)
        {
            var result = (
                    from food in worldState.Food
                    let distance = food.Position.Distance(wormPosition)
                    where distance < wormLifeStrength && distance < food.ExpiresIn
                    select (distance, food)
                )
                .ToList();
            result.Sort((a, b) => a.distance - b.distance);
            return result;
        }
    }
}
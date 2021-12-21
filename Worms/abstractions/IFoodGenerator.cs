using System.Collections.Generic;
using Worms.entities;

namespace Worms.abstractions
{
    public interface IFoodGenerator
    {
        List<Food> GenerateFood(World world);
    }
}
using System.Collections.Generic;

namespace Worms.abstractions
{
    public interface IFoodGenerator
    {
        List<Food> GenerateFood(World world);
    }
}
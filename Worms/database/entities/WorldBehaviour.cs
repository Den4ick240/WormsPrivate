using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Worms.database.entities
{
    public class WorldBehaviour
    {
        [Key] public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<FoodPoint> FoodPoints { get; set; }
    }
}
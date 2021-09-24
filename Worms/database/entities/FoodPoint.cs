using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Worms.database.entities
{
    [Owned]
    public class FoodPoint
    {
        [Key] public int Id { get; set; }

        public int Order { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}
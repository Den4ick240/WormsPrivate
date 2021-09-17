using System;
using System.Collections.Generic;
using Worms.abstractions;
using Worms.entities;

namespace Worms
{
    public class World
    {
        public List<Worm> Worms { init; get; }
        public List<Food> Foods { init; get; }
    }
}
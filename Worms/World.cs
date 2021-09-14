using System;
using System.Collections.Generic;

namespace Worms
{
    public class World
    {
        public List<AbstractWorm> Worms { init; get; }
        public List<Food> Foods { init; get; }
    }
}
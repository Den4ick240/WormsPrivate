using System;
using Worms.Domain;

namespace Worms.Web.Behaviour
{
    public class NameNotFoundException : Exception
    {
        public NameNotFoundException(string message) : base(message)
        {
        }
    }

    public abstract class AbstractWormBehaviour
    {
        public Response GetResponse(WorldState worldState, string name, int run, int step)
        {
            var worm = worldState.Worms.Find(w => w.Name.Equals(name));
            if (worm == null) throw new NameNotFoundException($"Name {name} was not found on the list of worms!");
            return GetResponse(worldState, worm, run, step);
        }

        protected abstract Response GetResponse(WorldState worldState, Worm worm, int run, int step);
    }
}
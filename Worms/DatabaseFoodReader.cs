using System.Linq;
using Worms.database;
using Worms.database.entities;

namespace Worms
{
    public class DatabaseFoodReader
    {
        private readonly DatabaseContext _dbCtx;
        public DatabaseFoodReader(DatabaseContext dbCtx)
        {
            _dbCtx = dbCtx;
        }

        public WorldBehaviour ReadWorldBehaviour(string name)
        {
            return _dbCtx.WorldBehaviours.First(behaviour => behaviour.Name == name);
        }
    }
}
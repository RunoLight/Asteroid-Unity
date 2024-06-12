using Asteroid.GameLogic.EntityManagement.Entity;
using Asteroid.GameLogic.SpawnStrategy;
using Factory;

namespace Asteroid.GameLogic.EntityManagement.Manager
{
    public class UfoManager : EntityManager<Ufo>
    {
        public UfoManager(AutoSpawnStrategySettings settings, IFactory<Ufo> entityFactory)
            : base(entityFactory, null)
        {
            SpawnStrategy = new AutoSpawnStrategy<Ufo>(this, settings);
        }
    }
}
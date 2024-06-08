using Asteroid.GameLogic.EntityManagement;

namespace Asteroid.GameLogic.SpawnStrategy
{
    public class AutoSpawnStrategy<TValue> : ISpawnStrategy<TValue>
    {
        private readonly IEntityManager<TValue> manager;
        private readonly AutoSpawnStrategySettings settings;

        private bool started;
        private float timeToNextSpawn;

        public AutoSpawnStrategy(IEntityManager<TValue> manager, AutoSpawnStrategySettings settings)
        {
            this.manager = manager;
            this.settings = settings;

            timeToNextSpawn = settings.TimeBetweenSpawns;
        }

        public void Enable()
        {
            started = true;

            for (int i = 0; i < settings.SpawnsAtStart; i++)
            {
                manager.CreateEntity();
            }
        }

        public void Disable()
        {
            started = false;
        }

        public void Tick(float deltaTime)
        {
            if (!started)
                return;

            if (manager.EntitiesCount > settings.MaxSpawns)
                return;

            timeToNextSpawn -= deltaTime;

            if (timeToNextSpawn > 0)
                return;

            timeToNextSpawn = settings.TimeBetweenSpawns;
            manager.CreateEntity();
        }

        public TValue Spawn()
        {
            return manager.CreateEntity();
        }
    }
    
    public class AutoSpawnStrategySettings
    {
        public readonly float TimeBetweenSpawns;
        public readonly int SpawnsAtStart;
        public readonly float MaxSpawns;

        public AutoSpawnStrategySettings(float maxSpawns, float timeBetweenSpawns, int spawnsAtStart)
        {
            MaxSpawns = maxSpawns;
            TimeBetweenSpawns = timeBetweenSpawns;
            SpawnsAtStart = spawnsAtStart;
        }
    }
}
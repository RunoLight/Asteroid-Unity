using Asteroid.GameLogic.EntityManagement;

namespace Asteroid.GameLogic.SpawnStrategy
{
    public class ManualSpawnStrategy<TValue> : ISpawnStrategy<TValue>
    {
        private readonly IEntityManager<TValue> manager;

        public ManualSpawnStrategy(IEntityManager<TValue> manager)
        {
            this.manager = manager;
        }

        public void Enable()
        {
        }

        public void Disable()
        {
        }

        public void Tick(float deltaTime)
        {
        }

        public TValue Spawn()
        {
            return manager.CreateEntity();
        }
    }
}
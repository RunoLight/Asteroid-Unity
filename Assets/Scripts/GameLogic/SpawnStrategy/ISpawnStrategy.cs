namespace Asteroid.GameLogic.SpawnStrategy
{
    public interface ISpawnStrategy<out TEntity>
    {
        void Enable();
        void Disable();
        TEntity Spawn();
        public void Tick(float deltaTime);
    }
}
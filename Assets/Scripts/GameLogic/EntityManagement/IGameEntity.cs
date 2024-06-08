using System;

namespace Asteroid.GameLogic.EntityManagement
{
    public interface IGameEntity<out TEntity>
    {
        public event Action<TEntity> Destroyed;

        public object EntityPresentation { get; }

        public void Tick(float deltaTime);
        public void FixedTick(float fixedDeltaTime);
        public void Destroy();
    }
}
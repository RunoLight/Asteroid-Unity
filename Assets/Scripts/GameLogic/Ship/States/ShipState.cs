using System;

namespace Asteroid.GameLogic.Ship.States
{
    public abstract class ShipState : IDisposable
    {
        public abstract void Tick(float deltaTime);

        public abstract void FixedTick(float fixedDeltaTime);

        public virtual void Start()
        {
            // optionally overridden
        }

        public virtual void Dispose()
        {
            // optionally overridden
        }
        
        public virtual void OnCollision()
        {
            // optionally overridden
        }
    }
}

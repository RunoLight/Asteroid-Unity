using UnityEngine;

namespace Asteroid.Presentation.Entity.Ship.Abstractions
{
    public interface IShipExplosion
    {
        public bool IsDestroyedViaLifetimeExpiration { get; }
        public void Setup(Vector2 position);
        public void Destroy();
    }
}
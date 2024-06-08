using UnityEngine;

namespace Asteroid.Presentation.Entity.Ship.Abstractions
{
    public interface IBrokenShip
    {
        public void Setup(Vector2 position, Quaternion rotation, Vector3 velocity);
        public void Destroy();
    }
}
using Asteroid.Presentation.Entity.Ship.Abstractions;
using UnityEngine;

namespace Asteroid.Presentation.Entity.Ship
{
    public class ShipExplosion : MonoBehaviour, IShipExplosion
    {
        public bool IsDestroyedViaLifetimeExpiration => lifeTimeLeft <= 0;
        
        private float lifeTimeLeft = LifeTime;

        private const float LifeTime = 0.3f;

        public void Setup(Vector2 position)
        {
            transform.position = position;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void Update()
        {
            lifeTimeLeft -= Time.deltaTime;
            if (lifeTimeLeft <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
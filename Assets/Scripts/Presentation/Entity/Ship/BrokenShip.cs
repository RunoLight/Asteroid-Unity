using Asteroid.Presentation.Entity.Ship.Abstractions;
using UnityEngine;

namespace Asteroid.Presentation.Entity.Ship
{
    public class BrokenShip : MonoBehaviour, IBrokenShip
    {
        private const float RotationSpeed = 30f;

        private Vector3 velocity;

        public void Setup(Vector2 position, Quaternion rotation, Vector3 velocityForce)
        {
            transform.position = position;
            transform.rotation = rotation;
            velocity = velocityForce;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void Update()
        {
            transform.position += velocity * Time.deltaTime;
            transform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);
        }
    }
}
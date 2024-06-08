using Asteroid.Presentation.Entity.Abstractions;
using UnityEngine;

namespace Asteroid.GameLogic.EntityManagement.Entity
{
    public class Asteroid : GameEntity<Asteroid, IAsteroidPresentation>
    {
        /// <summary>
        /// If asteroid is destroyed - it spawns two asteroid chunks. <br/>
        /// If asteroid chunk is destroyed - it does not spawn anything.
        /// </summary>
        public bool IsAsteroidChunk { get; private set; }

        private const float RotationSpeed = 30f;

        public void ConstructAsteroidChunk(Vector2 position, Vector2 velocity, float scale)
        {
            IsAsteroidChunk = true;
            Position = position;
            Velocity = velocity;
            Scale = scale;
        }

        public override void Initialize()
        {
            base.Initialize();
            Presentation.OnLethalCollided += OnLethalCollision;
        }

        public override void Destroy()
        {
            Presentation.OnLethalCollided -= OnLethalCollision;
            Presentation.Destroy();
            base.Destroy();
        }

        private void OnLethalCollision()
        {
            Destroy();
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            Presentation.RotateAroundSelf(RotationSpeed * fixedDeltaTime);
            Presentation.Position += Velocity * fixedDeltaTime;
        }
    }
}
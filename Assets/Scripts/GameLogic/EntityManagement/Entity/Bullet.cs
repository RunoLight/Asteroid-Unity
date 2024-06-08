using Asteroid.Presentation.Entity.Abstractions;
using UnityEngine;

namespace Asteroid.GameLogic.EntityManagement.Entity
{
    public class Bullet : GameEntity<Bullet, IBulletPresentation>
    {
        private float lifetimeRemaining = TotalLifeTime;

        private const float Speed = 16f;
        private const float TotalLifeTime = 3f;
        private const float DefaultSpriteRotationAngle = -90;

        public void Setup(Vector2 position, Quaternion rotation)
        {
            Position = position;

            Presentation.Rotation = rotation;
            Presentation.RotateAroundSelf(DefaultSpriteRotationAngle);

            Velocity = rotation * Vector2.up * Speed;

            Presentation.SetActive(true);
        }

        public override void Initialize()
        {
            base.Initialize();
            lifetimeRemaining = TotalLifeTime;
            Presentation.OnLethalCollided += OnLethalCollision;
        }

        public override void Destroy()
        {
            // No presentation.Destroy(); because it's being object pooled
            Presentation.OnLethalCollided -= OnLethalCollision;
            Presentation.SetActive(false);
            base.Destroy();
        }

        private void OnLethalCollision()
        {
            Destroy();
        }

        public override void Tick(float deltaTime)
        {
            base.Tick(deltaTime);

            lifetimeRemaining -= deltaTime;
            if (lifetimeRemaining <= 0f)
            {
                Destroy();
            }
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            base.FixedTick(fixedDeltaTime);
            Presentation.Position += Velocity * fixedDeltaTime;
        }
    }
}
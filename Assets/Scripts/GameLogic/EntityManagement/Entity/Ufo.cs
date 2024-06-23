using Asteroid.Presentation.Abstractions;
using Asteroid.Presentation.Entity.Abstractions;

namespace Asteroid.GameLogic.EntityManagement.Entity
{
    public class Ufo : GameEntity<Ufo, IUfoPresentation>
    {
        private IObjectPositionAdapter followingTargetPosition;

        private const float Speed = 2f;

        public Ufo Initialize(IObjectPositionAdapter target)
        {
            followingTargetPosition = target;
            return this;
        }

        public override void Initialize()
        {
            base.Initialize();
            Presentation.OnLethalCollided += OnLethalCollision;
        }

        public override void Destroy()
        {
            Presentation.Destroy();

            Presentation.OnLethalCollided -= OnLethalCollision;
            base.Destroy();
        }

        private void OnLethalCollision()
        {
            Destroy();
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            Velocity = (followingTargetPosition.Value - Presentation.Position).normalized * Speed;
            Presentation.Position += Velocity * fixedDeltaTime;
        }
    }
}
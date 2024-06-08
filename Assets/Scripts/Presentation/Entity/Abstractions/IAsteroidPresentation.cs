using System;

namespace Asteroid.Presentation.Entity.Abstractions
{
    public interface IAsteroidPresentation : IEntityPresentation
    {
        public event Action OnLethalCollided;
        public void Destroy();
        public void RotateAroundSelf(float degree);
    }
}
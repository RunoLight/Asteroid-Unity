using System;

namespace Asteroid.Presentation.Entity.Abstractions
{
    public interface IUfoPresentation : IEntityPresentation
    {
        public event Action OnLethalCollided;
        public void Destroy();
    }
}
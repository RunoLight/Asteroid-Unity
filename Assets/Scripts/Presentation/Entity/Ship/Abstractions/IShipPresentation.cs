using System;
using Asteroid.Presentation.Abstractions;

namespace Asteroid.Presentation.Entity.Ship.Abstractions
{
    public interface IShipPresentation : IObjectPositionAdapter, IObjectRotationAdapter
    {
        public event Action OnLethalCollided;
        void SetActive(bool value);
        void DisableParticlesForOneFrame();
        void EnableParticles(bool isActive);
    }
}
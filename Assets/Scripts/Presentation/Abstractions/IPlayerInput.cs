using System;

namespace Asteroid.Presentation.Abstractions
{
    public interface IPlayerInput
    {
        public event Action<bool> Forward;
        public event Action<bool> ActivateLaser;
        public event Action<float> Rotate;
    }
}
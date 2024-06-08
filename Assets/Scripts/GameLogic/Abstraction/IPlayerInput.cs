using System;

namespace Asteroid.GameLogic.Abstraction
{
    public interface IPlayerInput
    {
        public event Action<bool> Forward;
        public event Action<bool> ActivateLaser;
        public event Action<float> Rotate;
    }
}
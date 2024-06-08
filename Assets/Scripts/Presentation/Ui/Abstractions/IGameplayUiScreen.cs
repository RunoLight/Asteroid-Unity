using UnityEngine;

namespace Asteroid.Presentation.Ui.Abstractions
{
    public interface IGameplayUiScreen : IUiScreen
    {
        public void SetScore(int score);
        public void SetLaserCooldown(int charges, float progress01);
        public void SetInfo(Vector2 position, Quaternion rotation, Vector2 velocity);
    }
}
using Asteroid.Presentation.Ui.Abstractions;
using TMPro;
using UnityEngine;

namespace Asteroid.Presentation.Ui
{
    public class GameplayUiScreen : MonoBehaviour, IGameplayUiScreen
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text cooldownText;
        [SerializeField] private TMP_Text infoText;

        public void SetScore(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        public void SetLaserCooldown(int charges, float progress01)
        {
            cooldownText.text = $"Charges: {charges}, Laser overheat: {100 * progress01:00}%";
        }

        public void SetInfo(Vector2 position, Quaternion rotation, Vector2 velocity)
        {
            infoText.text = $"Pos: {position}, Angle:{rotation.eulerAngles.z:00}, Velocity: {velocity}";
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}
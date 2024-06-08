using Asteroid.Presentation.Ui.Abstractions;
using TMPro;
using UnityEngine;

namespace Asteroid.Presentation.Ui
{
    public class GameOverUiScreen : MonoBehaviour, IGameOverUiScreen
    {
        [SerializeField] private TMP_Text scoreText;

        public void SetScore(int score)
        {
            scoreText.text = $"Score: {score}";
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}
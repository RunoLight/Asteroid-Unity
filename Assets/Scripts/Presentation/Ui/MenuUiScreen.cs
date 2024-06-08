using Asteroid.Presentation.Ui.Abstractions;
using UnityEngine;

namespace Asteroid.Presentation.Ui
{
    public class MenuUiScreen : MonoBehaviour, IUiScreen
    {
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}
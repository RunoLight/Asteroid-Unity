using Asteroid.Presentation.Entity.Ship.Abstractions;
using Asteroid.Presentation.Marker;
using UnityEngine;

namespace Asteroid.Presentation.Entity.Ship
{
    public class LaserBeamPresentation : MonoBehaviour, IDestroysEnemies, ILaserBeamPresentation
    {
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}
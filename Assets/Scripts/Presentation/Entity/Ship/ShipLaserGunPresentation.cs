using Asteroid.Presentation.Entity.Ship.Abstractions;
using UnityEngine;

namespace Asteroid.Presentation.Entity.Ship
{
    public class ShipLaserGunPresentation : MonoBehaviour, IShipLaserGunPresentation
    {
        [SerializeField] private LaserBeamPresentation laserBeamPresentation;

        private ILaserBeamPresentation LaserBeam => laserBeamPresentation;

        public void SetLaserBeamActive(bool isActive)
        {
            LaserBeam.SetActive(isActive);
        }
    }
}
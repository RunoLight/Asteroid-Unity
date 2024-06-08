using Asteroid.Presentation.Entity.Ship.Abstractions;
using UnityEngine;

namespace Asteroid.Presentation.Entity.Ship
{
    public class ShipLaserGunPresentation : MonoBehaviour, IShipLaserGun
    {
        private ILaserBeamPresentation LaserBeam => laserBeamPresentation;

        [SerializeField] private LaserBeamPresentation laserBeamPresentation;

        public void SetLaserBeamActive(bool isActive)
        {
            LaserBeam.SetActive(isActive);
        }
    }
}